using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.NUnit;

public class SampleTests
{
    private static class Names
    {
        public static IEnumerable<Scenario<ImmutableArray<string>>> AllCases => new[]
        {
            ImmutableArray.Create<string>("cammy", "dhalsim").AsScenario("all lowercase")
        };
    }

    [Test]
    public void Names_should_get_capitalized(
        // Could also be used with TestCaseSource in this simple example, but the original use case
        // of scenarios is to be used with ValueSource, e.g. when providing combinations of values
        // to test method parameters via multiple value sources
        [ValueSource(typeof(Names), nameof(Names.AllCases))] Scenario<ImmutableArray<string>> names)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
