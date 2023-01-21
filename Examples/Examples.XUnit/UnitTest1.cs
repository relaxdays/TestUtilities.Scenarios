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
    }

    // TODO
    // [Theory]
    // [MemberData(Names.All)]
    // public void Test1()
    // {
    //
    // }
}
