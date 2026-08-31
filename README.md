[![](https://img.shields.io/nuget/v/soenneker.instantly.leads.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.instantly.leads/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.instantly.leads/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.instantly.leads/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.instantly.leads.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.instantly.leads/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.instantly.leads/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.instantly.leads/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Instantly.Leads

Add leads to campaigns, find or search them by email, and delete matching leads.

## Install

```bash
dotnet add package Soenneker.Instantly.Leads
```

## Configure and register

```json
{
  "Instantly": {
    "ApiKey": "<API key>",
    "LogEnabled": false
  }
}
```

```csharp
using Soenneker.Instantly.Leads.Registrars;

services.AddInstantlyLeadUtilAsScoped();
```

The scoped lead service deliberately uses the singleton generated-client provider. Use `AddInstantlyLeadUtilAsSingleton()` when the operation layer should also live for the application lifetime.

## Add a lead

```csharp
using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Instantly.OpenApiClient.Models;

var request = new CreateLeadRequest
{
    Email = "person@example.com",
    FirstName = "Morgan",
    LastName = "Lee"
};

Lead? created = await leads.Add(
    request,
    campaignId,
    cancellationToken);
```

This overload sets `request.Campaign` and lowercases `request.Email` before sending it, so it mutates the supplied request. Use `Add(request, cancellationToken)` when `Campaign` is already populated; that overload also lowercases the email.

## Find leads

```csharp
Lead? first = await leads.GetByEmail(
    "person@example.com",
    campaignId,
    cancellationToken);

List<Lead>? matches = await leads.Search(
    "person@example.com",
    campaignId,
    cancellationToken);
```

Omit `campaignId` to search across campaigns. `GetByEmail` requests one result and returns the first match; `Search` returns the matching list from Instantly.

## Delete leads

```csharp
Lead? firstDeleted = await leads.Delete(
    ["person@example.com"],
    campaignId,
    cancellationToken);
```

`Delete` first resolves matching leads, then deletes every returned lead that has an ID. Omitting `campaignId` expands that destructive operation across all campaigns. The return value is the first matched lead, not a count of successful deletions; an exception can leave a partially completed batch if a later deletion fails.

API failures are surfaced to the caller. Nullable results indicate that Instantly returned no response body or no matching lead.
