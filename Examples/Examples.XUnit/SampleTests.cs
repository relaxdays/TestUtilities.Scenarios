using System.Diagnostics.CodeAnalysis;
using Examples.Common;
using Relaxdays.TestUtilities.Scenarios;

namespace Examples.XUnit;

public class SampleTests
{
    private static class Players
    {
        private static IEnumerable<Scenario<Player>> AllCases
            => new Player[] { new("dee jay"), new("blanka") }
                .Select(player => player.AsScenario(player.Name));


        // Needed because we can't use Names.AllCases directly, as it will produce error xUnit1019
        //      See: https://xunit.net/xunit.analyzers/rules/xUnit1019
        public static IEnumerable<object[]> AllAsObjects => AllCases.Select(scenario => new object[] { scenario });
    }

    [Theory]
    [MemberData(nameof(Players.AllAsObjects), MemberType = typeof(Players))]
    [SuppressMessage("Roslyn", "xUnit1026",
        Justification = "We don't need to use the method parameter to showcase Scenario usage")]
    public void Player_names_should_get_capitalized(Scenario<Player> playerScenario)
    {
        // Some business logic failing to capitalize names
        Assert.Fail("player names should get capitalized");
    }
}
