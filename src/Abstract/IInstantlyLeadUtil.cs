using System.Collections.Generic;
using Soenneker.Instantly.Leads.Requests;
using Soenneker.Instantly.Leads.Responses;
using System.Threading.Tasks;
using Soenneker.Instantly.Leads.Requests.Partials;

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
    /// <returns>A task that represents the asynchronous operation. The task result contains the response for the add operation, or null if the operation fails.</returns>
    ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyLeadRequest lead, string campaignId);

    /// <summary>
    /// Adds multiple leads based on the provided request details.
    /// </summary>
    /// <param name="request">The request containing the details for adding multiple leads.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response for the bulk add operation, or null if the operation fails.</returns>
    ValueTask<InstantlyAddLeadsResponse?> Add(InstantlyAddLeadsRequest request);

    /// <summary>
    /// Searches for leads by email in a safe manner, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains a list of matching leads, or null if the operation fails.</returns>
    ValueTask<List<InstantlySearchLeadResponse>?> SearchSafe(string email, string? campaignId = null);

    /// <summary>
    /// Searches for leads by email, with optional filtering by campaign ID.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign to filter the search. Default is null, searching across all campaigns.</param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains a list of matching leads, or null if the operation fails.</returns>
    ValueTask<List<InstantlySearchLeadResponse>?> Search(string email, string? campaignId = null);

    /// <summary>
    /// Deletes specified emails from a campaign or globally, with an option to delete all leads from the company.
    /// </summary>
    /// <param name="emails">The list of email addresses to delete.</param>
    /// <param name="deleteAllFromCompany">Indicates whether to delete all leads from the company. Default is false.</param>
    /// <param name="campaignId">Optional. The unique identifier of the campaign from which leads will be deleted. Default is null, deleting across all campaigns.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the response for the delete operation, or null if the operation fails.</returns>
    ValueTask<InstantlyOperationResponse?> Delete(List<string> emails, bool deleteAllFromCompany = false, string? campaignId = null);
}
