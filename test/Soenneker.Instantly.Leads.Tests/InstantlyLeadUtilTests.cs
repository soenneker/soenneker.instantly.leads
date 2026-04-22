using System.Collections.Generic;
using AwesomeAssertions;
using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;
using Soenneker.Instantly.OpenApiClient.Models;

namespace Soenneker.Instantly.Leads.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class InstantlyLeadUtilTests : HostedUnitTest
{
    private readonly IInstantlyLeadUtil _util;

    public InstantlyLeadUtilTests(Host host) : base(host)
    {
        _util = Resolve<IInstantlyLeadUtil>(true);
    }

    [Test]
    public void Default()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async ValueTask Search()
    {
        List<Def11>? result = await _util.Search("", null, CancellationToken);
        result.Should().NotBeNullOrEmpty();
    }
}
