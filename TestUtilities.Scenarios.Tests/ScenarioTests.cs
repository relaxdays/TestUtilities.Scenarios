using FluentAssertions;
using NUnit.Framework.Internal;

namespace Relaxdays.TestUtilities.Scenarios.Tests;

[TestOf(typeof(Scenario<>))]
[TestOf(typeof(Scenario))]
public class ScenarioTests
{
    // ----------------------------------------------------------------------------------------------------- //
    //                                          Data                                                         //
    // ----------------------------------------------------------------------------------------------------- //

    private static class ScenarioCreations
    {
        public static IEnumerable<TestCaseParameters> FromDataWithArbitraryDescription => new[]
        {
            new TestCaseData(new Func<string, Scenario<string>>(data => new(data)))
                .SetArgDisplayNames("Scenario constructor"),

            new TestCaseData(new Func<string, Scenario<string>>(data => data.AsScenario()))
                .SetArgDisplayNames("Extension method, without description"),

            new TestCaseData(new Func<string, Scenario<string>>(data => data.AsScenario("description")))
                .SetArgDisplayNames("Extension method, with description"),
            
            new TestCaseData(new Func<string, Scenario<string>>(data => data.AsScenario($"{data} description")))
                .SetArgDisplayNames("Extension method, with selected description")
        };

        public static IEnumerable<TestCaseParameters> FromDataWithDefaultDescription => new[]
        {
            new TestCaseData(new Func<string, Scenario<string>>(data => new(data)))
                .SetArgDisplayNames("Scenario constructor"),

            new TestCaseData(new Func<string, Scenario<string>>(data => data.AsScenario()))
                .SetArgDisplayNames("Extension method, without description")
        };

        public static IEnumerable<TestCaseParameters> FromDataAndDescription => new[]
        {
            new TestCaseData(new Func<string, string, Scenario<string>>(
                    (data, description) => new Scenario<string>(data).WithDescription(description)))
                .SetArgDisplayNames("Scenario constructor then WithDescription"),

            new TestCaseData(new Func<string, string, Scenario<string>>(
                    (data, description) => data.AsScenario().WithDescription(description)))
                .SetArgDisplayNames("Extension method then WithDescription"),

            new TestCaseData(new Func<string, string, Scenario<string>>(
                    (data, description) => data.AsScenario(description)))
                .SetArgDisplayNames("Extension method, description overload"),
            
            new TestCaseData(new Func<string, string, Scenario<string>>(
                    (data, description) => data.AsScenario(_ => description)))
                .SetArgDisplayNames("Extension method, description selector overload, returning constant description")
        };
    }
    
    public static class Combinations
    {
        public delegate Scenario<(string First, string Second)> PairingCombination(
            Scenario<string> first, Scenario<string> second);

        public static IEnumerable<TestCaseParameters> PairingScenarios => new[]
        {
            new TestCaseData(new PairingCombination((first, second) => first.CombinedWith(second)))
                .SetArgDisplayNames($"Member function {nameof(Scenario<string>.CombinedWith)}"),

            new TestCaseData(new PairingCombination(Scenario.Combine))
                .SetArgDisplayNames($"Static function {nameof(Scenario.Combine)}")
        };
    }

    // ----------------------------------------------------------------------------------------------------- //
    //                                   Configuration & Helper                                              //
    // ----------------------------------------------------------------------------------------------------- //

    private static void StringRepresentationShouldBeDescription<TData>(
        Scenario<TData> scenario, string description, string because = "", params object[] becauseArgs)
        => scenario.ToString().Should().Be($"\"{description}\"", because, becauseArgs);

    private static void StringRepresentationsShouldBeEqual(
            Scenario<string> scenario, Scenario<string> otherScenario, string because = "", params object[] becauseArgs)
        // => StringRepresentationShouldBeDescription(scenario, otherScenario.ToString(), because, becauseArgs);
        => scenario.ToString().Should().Be(otherScenario.ToString(), because, becauseArgs);

    // ----------------------------------------------------------------------------------------------------- //
    //                                              Tests                                                    //
    // ----------------------------------------------------------------------------------------------------- //

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithArbitraryDescription))]
    [Description("A scenario is created from arbitrary data. It should then contain that data.")]
    public void Scenarios_wrap_data(Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";

        // Act
        var scenario = scenarioCreation(data);

        // Assert
        scenario.Data.Should().Be(data);
    }

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataAndDescription))]
    [Description("A description is set for a scenario. The string representation of the scenario "
                 + "should then be that description.")]
    public void Description_sets_string_representation(Func<string, string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        const string description = "description";

        // Act
        var scenario = scenarioCreation(data, description);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(scenario, description);
    }

    // TODO: Test for data with ToString that returns null
    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithDefaultDescription))]
    [Description("""
        A scenario is created without setting a description. The description should then default to the data's string
        representation.
        """)]
    public void Default_description_is_data_to_string(Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";

        // Act
        var scenario = scenarioCreation(data);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(scenario, data);
    }

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataAndDescription))]
    [Description("The description of a scenario with arbitrary non empty description is transformed."
                 + "The string representation of the scenario should then be that transformed description.")]
    public void WithTransformedDescription_transforms_existing_description(
        Func<string, string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        const string description = "description";

        string TransformDescription(string originalDescription) => $"transformed {originalDescription}";
        var scenario = scenarioCreation(data, description);

        // Act
        var scenarioWithTransformedDescription = scenario.WithTransformedDescription(TransformDescription);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(
            scenarioWithTransformedDescription,
            TransformDescription(description));
    }

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataAndDescription))]
    [Description("The description of a scenario with arbitrary non empty description is appended to. The string "
                 + "representation of the scenario should then be the original description with the appendage.")]
    public void WithDescriptionAppendage_appends_to_existing_description(
        Func<string, string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        const string description = "description";
        const string appendage = "appendage";

        var scenario = scenarioCreation(data, description);

        // Act
        var scenarioWithTransformedDescription = scenario.WithDescriptionAppendage(appendage);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(
            scenarioWithTransformedDescription,
            $"{description} {appendage}");
    }

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithArbitraryDescription))]
    [Description("""
        Data of a scenario is transformed. It should then contain that transformed data, keeping the original
        description.
        """)]
    public void WithTransformedData_transforms_existing_data(Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        string TransformData(string originalData) => $"transformed {originalData}";

        var scenario = scenarioCreation(data);

        // Act
        var scenarioWithTransformedData = scenario.WithTransformedData(TransformData);

        // Assert
        scenarioWithTransformedData.Data.Should().Be(TransformData(data));
        StringRepresentationsShouldBeEqual(
            scenario,
            scenarioWithTransformedData,
            "transforming data should not alter the description");
    }

    // TODO: Use other types than string as well
    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithArbitraryDescription))]
    [Description(
        """
        A scenario is created from arbitrary data. It should then be implicitly convertible to the type of the wrapped
        data, returning the wrapped data on conversion.
        """)]
    public void Scenarios_are_implicitly_convertible_to_the_wrapped_type(
        Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        var scenario = scenarioCreation(data);

        // Act
        Func<string> implicitConversion = () => scenario;

        // Assert
        implicitConversion.Should().NotThrow(
            "a scenario should be implicitly convertible to the type of the wrapped data");

        var dataFromConversion = implicitConversion();
        dataFromConversion.Should().Be(data, "conversion should return the wrapped data");
    }

    [Test]
    [Description("""
        A scenario is combined with another scenario. The resulting scenario should then have data and description
        constructed from the specified selector functions.
        """)]
    public void CombinedWith_uses_specified_selector_functions()
    {
        // Arrange
        const string playerData = "Ibuki";
        const string playerDescription = "The player";
        var player = playerData.AsScenario(playerDescription);

        const string stageData = "Amazon River Basin";
        const string stageDescription = "The stage";
        var stage = stageData.AsScenario(stageDescription);

        string ResultDataSelector(string first, string second) => $"{first} @ {second}";
        string ResultDescriptionSelector(string first, string second) => $"{first} ; {second}";

        // Act
        var playerAndStage = player.CombinedWith(stage, ResultDataSelector, ResultDescriptionSelector);

        // Assert
        playerAndStage.Data.Should().Be(
            ResultDataSelector(playerData, stageData),
            "result data selector should be used to create result data");
        
        StringRepresentationShouldBeDescription(
            playerAndStage,
            ResultDescriptionSelector(playerDescription, stageDescription));
    }

    [TestCaseSource(typeof(Combinations), nameof(Combinations.PairingScenarios))]
    [Description("""
        A scenario is combined with another scenario without specifying selector functions. The resulting scenario
        should then have data and description paired up from the input scenarios.
        """)]
    public void Combinations_without_selector_functions_pair_up_data_and_description(
        Combinations.PairingCombination pairingCombination)
    {
        // Arrange
        const string playerData = "Vega";
        const string playerDescription = "The player";
        var player = playerData.AsScenario(playerDescription);

        const string stageData = "Jurassic Era Research Facility";
        const string stageDescription = "The stage";
        var stage = stageData.AsScenario(stageDescription);

        // Act
        var playerAndStage = pairingCombination(player, stage);

        // Assert
        playerAndStage.Data.First.Should().Be(playerData);
        playerAndStage.Data.Second.Should().Be(stageData);
        
        StringRepresentationShouldBeDescription(
            playerAndStage,
            $"({playerDescription}, {stageDescription})");
    }
}
