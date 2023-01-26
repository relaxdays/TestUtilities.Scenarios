using System.Collections.Immutable;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.MSTest;

[TestClass]
public class SampleTests
{
    private static class Names
    {
        private static IEnumerable<Scenario<ImmutableArray<string>>> AllCases => new[]
        {
            ImmutableArray.Create<string>("e.honda", "chun-li").AsScenario("all lowercase")
        };
        
        // Needed because we can't use Names.AllCases directly, as it will produce the following error message:
        // Value cannot be null. (Parameter 'Property or method AllCases on
        //      Examples.MSTest.SampleTests+Names does not return IEnumerable<object[]>.')
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[]{ scenario });
    }
    
    [TestMethod]
    [DynamicData(nameof(Names.AllCasesAsObjects), typeof(Names))]
    public void Names_should_get_capitalized(Scenario<ImmutableArray<string>> names)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
