using System.Text.Json.Serialization;

namespace Soenneker.Instantly.Leads.Responses;

public record InstantlyOperationResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
}