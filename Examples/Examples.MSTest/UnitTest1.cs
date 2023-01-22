using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.MSTest;

[TestClass]
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
    
    // TODO:
    [TestMethod]
    [DynamicData(nameof(Names.AllAsObjects), typeof(Names))]
    public void TestMethod1(Scenario<ImmutableArray<string>> names)
    {
        Assert.Fail();
    }
}
