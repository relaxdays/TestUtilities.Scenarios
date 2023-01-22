using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.XUnit;

public class UnitTest1
{
    private static class Names
    {
        public static IEnumerable<Scenario<ImmutableArray<string>>> All => new[]
        {
            ImmutableArray.Create<string>("first", "last").AsScenario("all lower")
        };

        public static IEnumerable<object[]> AllAsObjects => All.Select(scenario => new object[]{ scenario });
    }

    [Theory]
    [MemberData(nameof(Names.AllAsObjects), MemberType = typeof(Names))]
    public void Test1(Scenario<ImmutableArray<string>> names)
    {
        Assert.Fail("failure");
    }
}
