using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Soenneker.Instantly.OpenApiClient.Models;

namespace Soenneker.Instantly.Leads.Abstract;

/// <summary>
/// Adds, finds, searches, and deletes Instantly leads.
/// </summary>
public interface IInstantlyLeadUtil
{
    /// <summary>
    /// Adds a single lead to the specified campaign.
    /// </summary>
    /// <param name="lead">The lead request containing the lead's details.</param>
    /// <param name="campaignId">The unique identifier of the campaign to which the lead will be added.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created lead, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<Lead?> Add(CreateLeadRequest lead, string campaignId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a single lead using the campaign already set on the request.
    /// </summary>
    /// <param name="request">The lead and campaign details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created lead, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<Lead?> Add(CreateLeadRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for leads by email, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching leads, or <see langword="null"/> when the API returns no body.</returns>
    ValueTask<List<Lead>?> Search(string email, string? campaignId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes leads matching the specified email addresses from one campaign or across all campaigns.
    /// </summary>
    /// <param name="emails">The list of email addresses to delete.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign from which leads will be deleted. Default is null, deleting across all campaigns.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first matched lead, or <see langword="null"/> when none were found.</returns>
    ValueTask<Lead?> Delete(List<string> emails, string? campaignId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single lead by email, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address of the lead to retrieve.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first matching lead, or <see langword="null"/> when none were found.</returns>
    ValueTask<Lead?> GetByEmail(string email, string? campaignId = null, CancellationToken cancellationToken = default);
}
