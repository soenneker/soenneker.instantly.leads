using Microsoft.Extensions.Logging;
using Soenneker.Instantly.Client.Abstract;
using Soenneker.Instantly.Leads.Abstract;
using System.Threading.Tasks;
using Soenneker.Instantly.Leads.Requests;
using System.Net.Http;
using Soenneker.Instantly.Leads.Responses;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Enumerable;
using System.Collections.Generic;
using System.Threading;
using Soenneker.Instantly.Leads.Requests.Partials;
using Soenneker.Extensions.Enumerable.String;
using Soenneker.Extensions.HttpClient;
using Soenneker.Extensions.ValueTask;
using Soenneker.Instantly.Client;

namespace Soenneker.Instantly.Leads;

/// <inheritdoc cref="IInstantlyLeadUtil"/>
public class InstantlyLeadUtil : IInstantlyLeadUtil
{
    private readonly IInstantlyClient _instantlyClient;
    private readonly ILogger<InstantlyLeadUtil> _logger;

    private readonly string _apiKey;
    private readonly bool _log;

    public InstantlyLeadUtil(IInstantlyClient instantlyClient, ILogger<InstantlyLeadUtil> logger, IConfiguration config)
    {
        _instantlyClient = instantlyClient;
        _logger = logger;

        _apiKey = config.GetValueStrict<string>("Instantly:ApiKey");
        _log = config.GetValue<bool>("Instantly:LogEnabled");
    }

    public async ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyLeadRequest lead, string campaignId, CancellationToken cancellationToken = default)
    {
        var request = new InstantlyAddLeadsRequest
        {
            CampaignId = campaignId,
            ApiKey = _apiKey,
            Leads = [lead]
        };

        return await Add(request, cancellationToken).NoSync();
    }

    public async ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyAddLeadsRequest request, CancellationToken cancellationToken = default)
    {
        foreach (InstantlyLeadRequest lead in request.Leads)
        {
            lead.Email = lead.Email.ToLowerInvariantFast();
        }

        if (_log)
            _logger.LogDebug("Adding leads ({emails}) to Instantly campaign ({CampaignId})...", request.Leads.ToCommaSeparatedString(), request.CampaignId);

        if (request.ApiKey.IsNullOrEmpty())
            request.ApiKey = _apiKey;

        HttpClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        InstantlyAddLeadsResponse? response = await client.SendWithRetryToType<InstantlyAddLeadsResponse>(HttpMethod.Post, InstantlyClient.BaseUri + "lead/add", request, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public async ValueTask<List<InstantlySearchLeadResponse>?> Search(string email, string? campaignId = null, CancellationToken cancellationToken = default)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Searching for lead from Instantly with email ({email}) and campaign ({CampaignId})...", email, campaignId);

        string url = InstantlyClient.BaseUri + $"lead/get?api_key={_apiKey}&email={email}";

        if (campaignId.Populated())
        {
            campaignId = campaignId.ToLowerInvariantFast();
            url += $"&campaign_id={campaignId}";
        }

        HttpClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        List<InstantlySearchLeadResponse>? response = await client.SendToType<List<InstantlySearchLeadResponse>>(url, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public async ValueTask<InstantlyOperationResponse?> Delete(List<string> emails, bool deleteAllFromCompany = false, string? campaignId = null, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogWarning("Deleting leads from Instantly with emails ({email}) and campaign ({CampaignId})...", emails.ToCommaSeparatedString(), campaignId);

        var request = new InstantlyDeleteLeadsRequest
        {
            ApiKey = _apiKey,
            DeleteList = emails,
            DeleteAllFromCompany = deleteAllFromCompany,
            CampaignId = campaignId
        };

        HttpClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        InstantlyOperationResponse? response = await client.SendWithRetryToType<InstantlyOperationResponse>(HttpMethod.Post, InstantlyClient.BaseUri + "lead/delete", request, cancellationToken: cancellationToken).NoSync();

        return response;
    }
}