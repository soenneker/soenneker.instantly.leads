using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Soenneker.Instantly.Leads.Requests;

public class InstantlyDeleteLeadsRequest
{
    [Required]
    [JsonPropertyName("api_key")]
    public string ApiKey { get; set; } = default!;

    /// <summary>
    /// Optional campaign id. if not provided, matching leads will be deleted across all campaigns.
    /// </summary>
    [JsonPropertyName("campaign_id")]
    public string? CampaignId { get; set; }

    /// <summary>
    /// Set to true if you would like to remove all leads with matching domains
    /// </summary>
    [JsonPropertyName("delete_all_from_company")]
    public bool DeleteAllFromCompany { get; set; }

    [Required]
    [JsonPropertyName("delete_list")]
    public List<string> DeleteList { get; set; } = default!;
}