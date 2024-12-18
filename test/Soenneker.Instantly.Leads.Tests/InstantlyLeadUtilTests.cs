using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Instantly.Leads.Requests;
using Soenneker.Instantly.Leads.Requests.Partials;
using Soenneker.Tests.FixturedUnit;
using Xunit;

using Microsoft.Extensions.Configuration;
using Soenneker.Facts.Local;
using Soenneker.Instantly.Leads.Responses;

namespace Soenneker.Instantly.Leads.Tests;

[Collection("Collection")]
public class InstantlyLeadUtilTests : FixturedUnitTest
{
    private readonly IInstantlyLeadUtil _util;

    public InstantlyLeadUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IInstantlyLeadUtil>(true);
    }

    [LocalFact]
    public async Task Add_should_add_lead()
    {
        var config = Resolve<IConfiguration>();

        var lead = AutoFaker.Generate<InstantlyLeadRequest>();
        lead.Email = Faker.Internet.Email();

        var request = new InstantlyAddLeadsRequest
        {
            CampaignId = "89aa44d1-6f25-43af-8640-5a1cc1be1323",
            Leads =
            [
                lead
            ]
        };

        InstantlyAddLeadsResponse? response = await _util.Add(request);
        response!.Status.Should().Be("success");

        List<InstantlySearchLeadResponse>? result = await _util.Search(request.Leads[0].Email, request.CampaignId);
        result!.First().Contact.Should().Be(request.Leads[0].Email);
    }

    [LocalFact]
    public async Task Search_should_return_lead()
    {
        List<InstantlySearchLeadResponse>? response = await _util.Search("blah@blah.com");
        response.Should().NotBeNull();
    }
}