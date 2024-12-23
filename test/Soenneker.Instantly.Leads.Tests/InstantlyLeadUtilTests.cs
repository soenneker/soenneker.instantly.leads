using Soenneker.Instantly.Leads.Abstract;
using Soenneker.Tests.FixturedUnit;
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
}
