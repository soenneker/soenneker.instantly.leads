using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Instantly.Leads.Requests;
using Soenneker.Instantly.Leads.Requests.Partials;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.Configuration;
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

        var request = new InstantlyAddLeadsRequest
        {
            CampaignId = "c779a6b6-1558-4aae-bba9-fb2c55b720e9",
            ApiKey = config.GetValueStrict<string>("Instantly:ApiKey"),
            Leads =
            [
                AutoFaker.Generate<InstantlyLeadRequest>()
            ]
        };

        request.Leads[0].Email = Faker.Internet.Email();

        InstantlyAddLeadsResponse? response = await _util.Add(request);
        response!.Status.Should().Be("success");

        List<InstantlySearchLeadResponse>? result = await _util.Search(request.Leads[0].Email, request.CampaignId);
        result!.First().Contact.Should().Be(request.Leads[0].Email);
    }

    [LocalFact]
    public async Task Search_should_return_lead()
    {  
        List<InstantlySearchLeadResponse>? response = await _util.Search("antonette4@gmail.com");
        response.Should().NotBeNull();
    }
}
