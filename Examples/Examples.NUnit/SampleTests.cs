using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.NUnit;

public class SampleTests
{
    private static class Players
    {
        public static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("cammy"), new("dhalsim") }
                .Select(player => player.AsScenario(player.Name));
    }

    [Test]
    public void Player_names_should_get_capitalized(
        // Could also be used with TestCaseSource in this simple example, but the original use case of scenarios is to
        // be used with ValueSource, e.g. when providing combinations of values to test method parameters via multiple
        // value sources
        [ValueSource(typeof(Players), nameof(Players.AllCases))] Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail();
    }
}
