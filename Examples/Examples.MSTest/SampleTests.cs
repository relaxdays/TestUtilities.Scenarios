using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.MSTest;

[TestClass]
public class SampleTests
{
    private static class Players
    {
        private static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("e.honda"), new("chun-li") }
                .Select(player => player.AsScenario(player.Name));

        // Needed because we can't use Players.AllCases directly, as it will produce the following error message:
        // Value cannot be null. (Parameter 'Property or method AllCases on
        //      Examples.MSTest.SampleTests+Players does not return IEnumerable<object[]>.')
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }

    [TestMethod]
    [DynamicData(nameof(Players.AllCasesAsObjects), typeof(Players))]
    public void Player_names_should_get_capitalized(Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
