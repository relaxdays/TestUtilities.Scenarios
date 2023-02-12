using System.Diagnostics.CodeAnalysis;
using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.XUnit;

[SuppressMessage("Roslyn", "xUnit1026", Justification = """
    We don't need to use method parameters in these examples because we only care about how they are displayed.
    """)]
public class SampleTests
{
    // -------------------------------------------------------------------------------------------------------------- //
    //                                              Raw example                                                       //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersRaw
    {
        private static IEnumerable<Player> AllCases
            => new Player[] { new("rose"), new("fei long") };

        // Needed because member data sources must be of type IEnumerable<object[]>, so AllCases can't be used directly
        //      See: See: https://xunit.net/xunit.analyzers/rules/xUnit1019
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }

    [Theory]
    [MemberData(nameof(PlayersRaw.AllCasesAsObjects), MemberType = typeof(PlayersRaw))]
    public void XUnit_Player_names_should_get_capitalized_raw(Player player)
    {
        // Some business logic failing to capitalize names
        Assert.Fail("player names should get capitalized");
    }
    
    // -------------------------------------------------------------------------------------------------------------- //
    //                                            Scenario example                                                    //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersScenario
    {
        private static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("dee jay"), new("blanka") }
                .Select(player => player.AsScenario(player.Name));

        // Needed because member data sources must be of type IEnumerable<object[]>, so AllCases can't be used directly
        //      See: See: https://xunit.net/xunit.analyzers/rules/xUnit1019
        public static IEnumerable<object[]> AllCasesAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }

    [Theory]
    [MemberData(nameof(PlayersScenario.AllCasesAsObjects), MemberType = typeof(PlayersScenario))]
    public void XUnit_Player_names_should_get_capitalized_scenario(Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail("player names should get capitalized");
    }
}
