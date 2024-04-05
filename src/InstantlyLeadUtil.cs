using System;
using Microsoft.Extensions.Logging;
using Soenneker.Instantly.Client.Abstract;
using Soenneker.Instantly.Leads.Abstract;
using System.Threading.Tasks;
using Soenneker.Instantly.Leads.Requests;
using Soenneker.Utils.Json;
using System.Net.Http;
using Soenneker.Instantly.Leads.Responses;
using Soenneker.Extensions.Object;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Enumerable;
using System.Collections.Generic;
using Soenneker.Instantly.Leads.Requests.Partials;
using Soenneker.Extensions.Enumerable.String;

namespace Soenneker.Instantly.Leads;

/// <inheritdoc cref="IInstantlyLeadUtil"/>
public class InstantlyLeadUtil : IInstantlyLeadUtil
{
    private readonly IInstantlyClient _instantlyClient;
    private readonly ILogger<InstantlyLeadUtil> _logger;
    private const string _baseUrl = "https://api.instantly.ai/api/v1/";

    private readonly string _apiKey;
    private readonly bool _log;

    public InstantlyLeadUtil(IInstantlyClient instantlyClient, ILogger<InstantlyLeadUtil> logger, IConfiguration config)
    {
        _instantlyClient = instantlyClient;
        _logger = logger;

        _apiKey = config.GetValueStrict<string>("Instantly:ApiKey");
        _log = config.GetValue<bool>("Instantly:LogEnabled");
    }

    public ValueTask<InstantlyAddLeadsResponse?> AddSafe(InstantlyLeadRequest lead, string campaignId)
    {
        var request = new InstantlyAddLeadsRequest
        {
            CampaignId = campaignId,
            ApiKey = _apiKey,
            Leads = [lead]
        };

        return AddSafe(request);
    }

    public async ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyLeadRequest lead, string campaignId)
    {
        var request = new InstantlyAddLeadsRequest
        {
            CampaignId = campaignId,
            ApiKey = _apiKey,
            Leads = [lead]
        };

        return await Add(request);
    }

    public ValueTask<InstantlyAddLeadsResponse?> AddSafe(InstantlyAddLeadsRequest request)
    {
        try
        {
            return Add(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding leads to Instantly campaign ({CampaignId})", request.CampaignId);
            return default;
        }
    }

    public async ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyAddLeadsRequest request)
    {
        foreach (InstantlyLeadRequest lead in request.Leads)
        {
            lead.Email = lead.Email.ToLowerInvariantFast();
        }

        if (_log)
            _logger.LogDebug("Adding leads ({emails}) to Instantly campaign ({CampaignId})...", request.Leads.ToCommaSeparatedString(), request.CampaignId);

        if (request.ApiKey.IsNullOrEmpty())
            request.ApiKey = _apiKey;

        HttpClient client = await _instantlyClient.Get();

        var content = request.ToHttpContent();

        HttpResponseMessage httpResponse = await client.PostAsync(_baseUrl + "lead/add", content);

        string responseMessage = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            InstantlyAddLeadsResponse? response = null;

            try
            {
                response = JsonUtil.Deserialize<InstantlyAddLeadsResponse>(responseMessage);

                if (_log)
                    _logger.LogDebug("Added {LeadsUploaded} leads to Instantly campaign ({CampaignId})", response!.LeadsUploaded, request.CampaignId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception deserializing InstantlyAddLeadsResponse with content: {content}", responseMessage);
            }

            return response;
        }

        _logger.LogError("Non-success status code ({code}) from Instantly, content: {content}", (int) httpResponse.StatusCode, responseMessage);
        return null;
    }

    public ValueTask<List<InstantlySearchLeadResponse>?> SearchSafe(string email, string? campaignId = null)
    {
        try
        {
            return SearchSafe(email, campaignId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for lead from Instantly with email ({email}) and campaign ({CampaignId})", email, campaignId);
            return default;
        }
    }

    public async ValueTask<List<InstantlySearchLeadResponse>?> Search(string email, string? campaignId = null)
    {
        email = email.ToLowerInvariantFast();

        if (_log)
            _logger.LogDebug("Searching for lead from Instantly with email ({email}) and campaign ({CampaignId})...", email, campaignId);

        HttpClient client = await _instantlyClient.Get();

        string url = _baseUrl + $"lead/get?api_key={_apiKey}&email={email}";

        if (campaignId.Populated())
        {
            campaignId = campaignId.ToLowerInvariantFast();
            url += $"&campaign_id={campaignId}";
        }

        HttpResponseMessage httpResponse = await client.GetAsync(url);

        string responseMessage = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            List<InstantlySearchLeadResponse>? response = null;

            try
            {
                response = JsonUtil.Deserialize<List<InstantlySearchLeadResponse>>(responseMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception deserializing InstantlySearchLeadResponse with content: {content}", responseMessage);
            }

            return response;
        }

        _logger.LogError("Non-success status code ({code}) from Instantly with email ({email}) and campaign ({CampaignId}), content: {content}", (int)httpResponse.StatusCode, email, campaignId, responseMessage);
        return null;
    }

    public ValueTask<InstantlyOperationResponse?> DeleteSafe(List<string> emails, bool deleteAllFromCompany = false, string? campaignId = null)
    {
        try
        {
            return Delete(emails, deleteAllFromCompany, campaignId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting leads from Instantly with emails ({email}) and campaign ({CampaignId})", emails.ToCommaSeparatedString(), campaignId);
            return default;
        }
    }

    public async ValueTask<InstantlyOperationResponse?> Delete(List<string> emails, bool deleteAllFromCompany = false, string? campaignId = null)
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

        HttpClient client = await _instantlyClient.Get();

        HttpResponseMessage response = await client.PostAsync(_baseUrl + "lead/delete", request.ToHttpContent());

        string responseMessage = await response.Content.ReadAsStringAsync();

        var responseObj = JsonUtil.Deserialize<InstantlyOperationResponse>(responseMessage);

        return responseObj;
    }
}