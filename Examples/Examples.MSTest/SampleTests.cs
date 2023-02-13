using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.MSTest;

[TestClass]
public class SampleTests
{
    // -------------------------------------------------------------------------------------------------------------- //
    //                                              Raw example                                                       //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersRaw
    {
        private static IEnumerable<Player> AllCases
            => new Player[] { new("ryu"), new("m. bison") };

        // Needed because dynamic data sources must be of type IEnumerable<object[]>, so AllCases can't be used directly 
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }
    
    [TestMethod]
    [DynamicData(nameof(PlayersRaw.AllCasesAsObjects), typeof(PlayersRaw))]
    public void MSTest_Player_names_should_get_capitalized_raw(Player player)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }

    // -------------------------------------------------------------------------------------------------------------- //
    //                                            Scenario example                                                    //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersScenario
    {
        private static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("e.honda"), new("chun-li") }
                .Select(player => player.AsScenario(player.Name));

        // Needed because dynamic data sources must be of type IEnumerable<object[]>, so AllCases can't be used directly 
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }

    [TestMethod]
    [DynamicData(nameof(PlayersScenario.AllCasesAsObjects), typeof(PlayersScenario))]
    public void MSTest_Player_names_should_get_capitalized_scenario(Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
