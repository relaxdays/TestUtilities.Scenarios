using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.NUnit;

public class Tests
{
    private static class Names
    {
        public static IEnumerable<Scenario<ImmutableArray<string>>> All => new[]
        {
            ImmutableArray.Create<string>("first", "last").AsScenario("all lower")
        };
    }

    [Test]
    public void Test1([ValueSource(typeof(Names), nameof(Names.All))] Scenario<ImmutableArray<string>> names)
    {
        Assert.Fail();
    }
}
