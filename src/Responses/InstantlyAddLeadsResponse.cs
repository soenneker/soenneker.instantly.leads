using System.Text.Json.Serialization;

namespace Soenneker.Instantly.Leads.Responses;

/// <summary>
/// Represents the response from an API call.
/// </summary>
public record InstantlyAddLeadsResponse
{
    /// <summary>
    /// Response status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    /// <summary>
    /// Number of leads sent with the request
    /// </summary>
    [JsonPropertyName("total_sent")]
    public int TotalSent { get; set; }

    /// <summary>
    /// Number of uploaded leads
    /// </summary>
    [JsonPropertyName("leads_uploaded")]
    public int LeadsUploaded { get; set; }

    /// <summary>
    /// Number of leads already in the campaign
    /// </summary>
    [JsonPropertyName("already_in_campaign")]
    public int AlreadyInCampaign { get; set; }

    /// <summary>
    /// Number of invalid email addresses in the sent request
    /// </summary>
    [JsonPropertyName("invalid_email_count")]
    public int InvalidEmailCount { get; set; }

    /// <summary>
    /// Number of duplicate emails in the sent request
    /// </summary>
    [JsonPropertyName("duplicate_email_count")]
    public int DuplicateEmailCount { get; set; }

    /// <summary>
    /// Remaining upload count for your workspace
    /// </summary>
    [JsonPropertyName("remaining_in_plan")]
    public int RemainingInPlan { get; set; }
}