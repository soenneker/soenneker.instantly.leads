using Microsoft.Extensions.Logging;
using Soenneker.Instantly.Leads.Abstract;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.String;
using System.Collections.Generic;
using Soenneker.Extensions.ValueTask;
using Soenneker.Instantly.ClientUtil.Abstract;
using Soenneker.Instantly.OpenApiClient;
using Soenneker.Instantly.OpenApiClient.Api.V2.Leads;
using System;
using Soenneker.Instantly.OpenApiClient.Models;
using Soenneker.Extensions.Task;
using System.Linq;
using Soenneker.Instantly.OpenApiClient.Api.V2.Leads.List;

namespace Soenneker.Instantly.Leads;

/// <inheritdoc cref="IInstantlyLeadUtil"/>
public sealed class InstantlyLeadUtil : IInstantlyLeadUtil
{
    private readonly IInstantlyOpenApiClientUtil _instantlyClient;
    private readonly ILogger<InstantlyLeadUtil> _logger;

    private readonly bool _log;

    public InstantlyLeadUtil(IInstantlyOpenApiClientUtil instantlyClient, ILogger<InstantlyLeadUtil> logger, IConfiguration config)
    {
        _instantlyClient = instantlyClient;
        _logger = logger;

        _log = config.GetValue<bool>("Instantly:LogEnabled");
    }

    public async ValueTask<Def11?> Add(LeadsPostRequestBody lead, string campaignId, CancellationToken cancellationToken = default)
    {
        lead.Campaign = Guid.Parse(campaignId);
        lead.Email = lead.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Adding lead ({email}) to Instantly campaign ({CampaignId})...", lead.Email, campaignId);

        InstantlyOpenApiClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        return await client.Api.V2.Leads.PostAsync(lead, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<Def11?> Add(LeadsPostRequestBody request, CancellationToken cancellationToken = default)
    {
        request.Email = request.Email?.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Adding lead ({email}) to Instantly campaign ({CampaignId})...", request.Email, request.Campaign);

        InstantlyOpenApiClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        return await client.Api.V2.Leads.PostAsync(request, config => { }, cancellationToken).NoSync();
    }

    public async ValueTask<Def11?> GetByEmail(string email, string? campaignId = null, CancellationToken cancellationToken = default)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Getting lead from Instantly with email ({email}) and campaign ({CampaignId})...", email, campaignId);

        InstantlyOpenApiClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        var requestBody = new ListPostRequestBody
        {
            Contacts = [email],
            Limit = 1
        };
        if (campaignId != null)
            requestBody.Campaign = Guid.Parse(campaignId);

        ListPostResponse? response = await client.Api.V2.Leads.List.PostAsListPostResponseAsync(requestBody, config => { }, cancellationToken).NoSync();

        return response?.Items?.FirstOrDefault();
    }

    public async ValueTask<List<Def11>?> Search(string email, string? campaignId = null, CancellationToken cancellationToken = default)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Searching for lead from Instantly with email ({email}) and campaign ({CampaignId})...", email, campaignId);

        InstantlyOpenApiClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        var requestBody = new ListPostRequestBody
        {
            Contacts = [email]
        };
        if (campaignId != null)
            requestBody.Campaign = Guid.Parse(campaignId);

        ListPostResponse? response = await client.Api.V2.Leads.List.PostAsListPostResponseAsync(requestBody, config => { }, cancellationToken).NoSync();

        return response?.Items;
    }

    public async ValueTask<Def11?> Delete(List<string> emails, string? campaignId = null, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogWarning("Deleting leads from Instantly with emails ({emails}) and campaign ({CampaignId})...", string.Join(", ", emails), campaignId);

        InstantlyOpenApiClient client = await _instantlyClient.Get(cancellationToken).NoSync();

        var requestBody = new ListPostRequestBody
        {
            Contacts = emails
        };

        if (campaignId != null)
            requestBody.Campaign = Guid.Parse(campaignId);

        ListPostResponse? response = await client.Api.V2.Leads.List.PostAsListPostResponseAsync(requestBody, config => { }, cancellationToken).NoSync();

        if (response?.Items == null || !response.Items.Any())
            return null;

        foreach (Def11 lead in response.Items)
        {
            if (lead.Id == null)
                continue;

            await client.Api.V2.Leads[lead.Id.Value].DeleteAsync(null, config => { }, cancellationToken).NoSync();
        }

        return response.Items.FirstOrDefault();
    }
}