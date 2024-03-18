using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Instantly.Leads.Responses;

public record InstantlySearchLeadResponse
{
    /// <summary>
    /// Timestamp created
    /// </summary>
    [JsonPropertyName("timestamp_created")]
    public DateTime TimestampCreated { get; set; }

    /// <summary>
    /// Campaign ID
    /// </summary>
    [JsonPropertyName("campaign")]
    public string? Campaign { get; set; }

    /// <summary>
    /// Lead's status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    /// <summary>
    /// Lead's email
    /// </summary>
    [JsonPropertyName("contact")]
    public string Contact { get; set; } = default!;

    /// <summary>
    /// If the lead has opened an email
    /// </summary>
    [JsonPropertyName("email_opened")]
    public bool EmailOpened { get; set; }

    /// <summary>
    /// If the lead has replied
    /// </summary>
    [JsonPropertyName("email_replied")]
    public bool EmailReplied { get; set; }

    [JsonPropertyName("lead_data")]
    public Dictionary<string, string>? LeadData { get; set; }

    [JsonPropertyName("verification_status")]
    public string? VerificationStatus { get; set; }

    [JsonPropertyName("email_clicked")]
    public bool? EmailClicked { get; set; }

    /// <summary>
    /// Name of the campaign
    /// </summary>
    [JsonPropertyName("campaign_name")]
    public string? CampaignName { get; set; }

    [JsonPropertyName("timestamp_last_contact")]
    public DateTime? TimestampLastContact { get; set; }

    [JsonPropertyName("timestamp_last_open")]
    public DateTime? TimestampLastOpen { get; set; }

    [JsonPropertyName("timestamp_last_reply")]
    public DateTime? TimestampLastReply { get; set; }

    [JsonPropertyName("timestamp_last_interest_change")]
    public DateTime? TimestampLastInterestChange { get; set; }

    [JsonPropertyName("email_open_count")]
    public int EmailOpenCount { get; set; }

    [JsonPropertyName("email_reply_count")]
    public int EmailReplyCount { get; set; }

    [JsonPropertyName("timestamp_last_click")]
    public DateTime? TimestampLastClick { get; set; }

    [JsonPropertyName("email_click_count")]
    public int EmailClickCount { get; set; }
}