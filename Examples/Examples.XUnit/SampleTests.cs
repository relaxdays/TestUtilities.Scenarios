using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

// xUnit1026: Theory methods should use all of their parameters
// Justification: We don't need to actually use names in the test case to showcase Scenario usage
#pragma warning disable xUnit1026

namespace Examples.XUnit;

public class SampleTests
{
    private static class Names
    {
        private static IEnumerable<Scenario<ImmutableArray<string>>> AllCases => new[]
        {
            ImmutableArray.Create<string>("dee jay", "blanka").AsScenario("all lowercase")
        };

        // Needed because we can't use Names.AllCases directly, as it will produce error xUnit1019
        //      See: https://xunit.net/xunit.analyzers/rules/xUnit1019
        public static IEnumerable<object[]> AllAsObjects => AllCases.Select(scenario => new object[]{ scenario });
    }

    [Theory]
    [MemberData(nameof(Names.AllAsObjects), MemberType = typeof(Names))]
    public void Names_should_get_capitalized(Scenario<ImmutableArray<string>> names)
    {
        // Some business logic failing to capitalize names
        Assert.Fail($"{nameof(names)} should get capitalized");
    }
}
