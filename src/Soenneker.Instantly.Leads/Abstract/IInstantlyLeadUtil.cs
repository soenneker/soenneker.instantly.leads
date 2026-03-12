using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Soenneker.Instantly.OpenApiClient.Api.V2.Leads;
using Soenneker.Instantly.OpenApiClient.Models;

namespace Soenneker.Instantly.Leads.Abstract;

/// <summary>
/// A .NET typesafe implementation of Instantly.ai's Lead API
/// </summary>
public interface IInstantlyLeadUtil
{
    /// <summary>
    /// Adds a single lead to the specified campaign.
    /// </summary>
    /// <param name="lead">The lead request containing the lead's details.</param>
    /// <param name="campaignId">The unique identifier of the campaign to which the lead will be added.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response for the add operation, or null if the operation fails.</returns>
    ValueTask<Def11?> Add(LeadsPostRequestBody lead, string campaignId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple leads based on the provided request details.
    /// </summary>
    /// <param name="request">The request containing the details for adding multiple leads.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response for the bulk add operation, or null if the operation fails.</returns>
    ValueTask<Def11?> Add(LeadsPostRequestBody request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for leads by email, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains a list of matching leads, or null if the operation fails.</returns>
    ValueTask<List<Def11>?> Search(string email, string? campaignId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes specified emails from a campaign or globally, with an option to delete all leads from the company.
    /// </summary>
    /// <param name="emails">The list of email addresses to delete.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign from which leads will be deleted. Default is null, deleting across all campaigns.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the response for the delete operation, or null if the operation fails.</returns>
    ValueTask<Def11?> Delete(List<string> emails, string? campaignId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single lead by email, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address of the lead to retrieve.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the lead if found, or null if not found.</returns>
    ValueTask<Def11?> GetByEmail(string email, string? campaignId = null, CancellationToken cancellationToken = default);
}