using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.Comparison;

public class SampleTests
{
    // Raw player values source
    private static class PlayersRaw
    {
        public static IEnumerable<Player> AllCases
            => new Player[] { new("ryu"), new("m. bison") };
    }

    // Player values as scenarios source
    private static class PlayersScenario
    {
        public static IEnumerable<Scenario<Player>> AllCases 
            => new Player[] { new("rose"), new("fei long") }
                .Select(player => player.AsScenario(player.Name));
    }
    
    // This test showcases that raw player value display strings don't let us easily determine which case was
    // problematic on a failed (or successful) test
    [Test]
    public void Player_names_should_get_capitalized_raw(
        [ValueSource(typeof(PlayersRaw), nameof(PlayersRaw.AllCases))]
        Player player)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }

    // This test showcases how wrapping raw player values in a scenario with a meaningful description makes for much
    // nicer and more meaningful display strings on a failed (or successful) test
    [Test]
    public void Player_names_should_get_capitalized_scenario(
        [ValueSource(typeof(PlayersScenario), nameof(PlayersScenario.AllCases))]
        Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
