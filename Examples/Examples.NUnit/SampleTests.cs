using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.NUnit;

public class SampleTests
{
    // -------------------------------------------------------------------------------------------------------------- //
    //                                              Raw example                                                       //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersRaw
    {
        public static IEnumerable<Player> AllCases
            => new Player[] { new("guile"), new("juri") };
    }

    [Test]
    [TestCaseSource(typeof(PlayersRaw), nameof(PlayersRaw.AllCases))]
    public void NUnit_Player_names_should_get_capitalized_raw(Player player)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }

    // -------------------------------------------------------------------------------------------------------------- //
    //                                            Scenario example                                                    //
    // -------------------------------------------------------------------------------------------------------------- //

    private static class PlayersScenario
    {
        public static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("cammy"), new("dhalsim") }
                .Select(player => player.AsScenario(player.Name));
    }

    [Test]
    // A NUnit-builtin alternative to scenarios (in this case) is using TestCaseSource with TestCaseParameters and
    // SetArgDisplayNames. See README for a more detail comparison of the different approaches.
    [TestCaseSource(typeof(PlayersScenario), nameof(PlayersScenario.AllCases))]
    public void NUnit_Player_names_should_get_capitalized_scenario(Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
