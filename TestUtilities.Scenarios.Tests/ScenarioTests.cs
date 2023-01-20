using FluentAssertions;
using NUnit.Framework.Internal;

namespace Relaxdays.TestUtilities.Scenarios.Tests;

[TestOf(typeof(Scenario<>))]
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
        };

        public static IEnumerable<TestCaseParameters> FromDataWithEmptyDescription => new[]
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
        };
    }

    // ----------------------------------------------------------------------------------------------------- //
    //                                   Configuration & Helper                                              //
    // ----------------------------------------------------------------------------------------------------- //

    private static void StringRepresentationShouldBeDescription(Scenario<string> scenario, string description)
        => scenario.ToString().Should().Be($"\"{description}\"");

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

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithEmptyDescription))]
    [Description("The description of a scenario with an empty description is transformed. The string "
                 + "representation of the scenario should then be the transformation of an empty string.")]
    public void WithTransformedDescription_transforms_empty_string_if_no_description_exists(
        Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";

        string TransformDescription(string originalDescription) => $"transformed {originalDescription}";
        var scenario = scenarioCreation(data);

        // Act
        var scenarioWithTransformedDescription = scenario.WithTransformedDescription(TransformDescription);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(
            scenarioWithTransformedDescription,
            TransformDescription(string.Empty));
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

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithEmptyDescription))]
    [Description("The description of a scenario with an empty description is appended to. The string "
                 + "representation of the scenario should then be an empty string with the appendage.")]
    public void WithDescriptionAppendage_appends_to_empty_string_if_no_description_exists(
        Func<string, Scenario<string>> scenarioCreation)
    {
        // Arrange
        const string data = "data";
        const string appendage = "appendage";

        var scenario = scenarioCreation(data);

        // Act
        var scenarioWithTransformedDescription = scenario.WithDescriptionAppendage(appendage);

        // Assert
        scenario.Data.Should().Be(data);
        StringRepresentationShouldBeDescription(
            scenarioWithTransformedDescription,
            $" {appendage}");
    }

    [TestCaseSource(typeof(ScenarioCreations), nameof(ScenarioCreations.FromDataWithArbitraryDescription))]
    [Description("Data of a scenario is transformed. It should then contain that transformed data.")]
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
    }
}
