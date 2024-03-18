using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Soenneker.Instantly.Leads.Requests.Partials;

namespace Soenneker.Instantly.Leads.Requests;

public class InstantlyAddLeadsRequest
{
    /// <summary>
    /// Your API key. Will be set in the request from <see cref="IConfiguration"/> if not provided.
    /// </summary>
    [Required]
    [JsonPropertyName("api_key")]
    public string ApiKey { get; set; } = default!;

    /// <summary>
    /// Your campaign's ID
    /// </summary>
    [Required]
    [JsonPropertyName("campaign_id")]
    public string CampaignId { get; set; } = default!;

    /// <summary>
    /// Skip lead if it exists in any campaigns in the workspace. Defaults to true.
    /// </summary>
    [JsonPropertyName("skip_if_in_workspace")]
    public bool SkipIfInWorkspace { get; set; } = true;

    [Required]
    [JsonPropertyName("leads")]
    public List<InstantlyLeadRequest> Leads { get; set; } = default!;
}