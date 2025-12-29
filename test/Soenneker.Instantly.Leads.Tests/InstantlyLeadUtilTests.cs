using System.Collections.Generic;
using AwesomeAssertions;
using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Tests.FixturedUnit;
using System.Threading.Tasks;
using Soenneker.Facts.Manual;
using Soenneker.Instantly.OpenApiClient.Models;
using Xunit;

namespace Soenneker.Instantly.Leads.Tests;

[Collection("Collection")]
public class InstantlyLeadUtilTests : FixturedUnitTest
{
    private readonly IInstantlyLeadUtil _util;

    public InstantlyLeadUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IInstantlyLeadUtil>(true);
    }

    [Fact]
    public void Default()
    {
    }

    [ManualFact]
    //[LocalFact]
    public async ValueTask Search()
    {
        List<Def11>? result = await _util.Search("", null, CancellationToken);
        result.Should().NotBeNullOrEmpty();
    }
}