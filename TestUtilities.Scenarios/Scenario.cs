using JetBrains.Annotations;

namespace Relaxdays.TestUtilities.Scenarios;

/// <summary>
/// Wrapper type for use with <c>NUnit</c> to enable better output of test argument descriptions.
/// </summary>
/// <remarks>
/// Primary use case is with <c>ValueSource</c>, so a description can be assigned to individual arguments.
/// For <c>TestCaseSource</c> this can be achieved with the <c>TestCaseParameters</c> provided by <c>NUnit</c>,
/// but that does not work once we need to combine our test cases via <c>ValueSource</c>.
/// </remarks>
/// <example>
///     <para>
///         To showcase the primary use case of scenarios, we're going to compare test name output generated
///         from a test using a <c>ValueSource</c>, with and without scenarios.
///     </para>
///     <para>
///         <b><i>Without scenarios</i></b><br/>
///         Say we have a value source that gives as a list of strings to use in a test, like so:
///         <br/>
///         <code>
///             Uploads_are_handled_correctly([ValueSource(typeof(Skus), nameof(Skus.All))] List&lt;string&gt; skus)
///         </code>
///         <br/>
///         We will then get test name output where we can't easily distinguish the separate cases:
///         <br/>
///         <code>
///             Failed Uploads_are_handled_correctly(System.Collections.Generic.List`1[System.String])
///             Failed Uploads_are_handled_correctly(System.Collections.Generic.List`1[System.String])
///         </code>
///     </para>
///     <para>
///         <b><i>With scenarios</i></b><br/>
///         The scenario mechanism on the other hand allows us to set meaningful descriptions for the test
///         arguments. We have to adjust the test case signature:
///         <br/>
///         <code>
///             Uploads_are_handled_correctly([ValueSource(typeof(Skus), nameof(Skus.All))] Scenario&lt;List&lt;string&gt;&gt; skus)
///         </code>
///         <br/>
///         Now we get a concise description (set accordingly where we create the scenarios) of the test arguments in
///         our output:
///         <br/>
///         <code>
///             Failed Uploads_are_handled_correctly("only valid skus")
///             Failed Uploads_are_handled_correctly("only invalid skus")
///         </code>
///     </para>
/// </example>
/// <param name="Data">The encapsulated test case data.</param>
/// <typeparam name="TData">The type of the test case data.</typeparam>
[PublicAPI]
public record Scenario<TData>(TData Data)
{
    private string Description { get; init; } = Data?.ToString() ?? string.Empty;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"\"{Description}\"";

    /// <summary>
    /// Provides a <paramref name="description"/> for the scenario.
    /// </summary>
    /// <param name="description">The description to provide.</param>
    /// <returns>
    ///     A scenario encapsulating <see cref="Data"/>, with the provided <paramref name="description"/>.
    /// </returns>
    [Pure]
    public Scenario<TData> WithDescription(string description) => this with { Description = description };

    /// <summary>
    /// Transforms the scenario's description (or <see cref="string.Empty"/>, if none has been set) by
    /// applying <paramref name="transformation"/> to it.
    /// </summary>
    /// <param name="transformation">The transformation to apply to the description.</param>
    /// <returns>
    ///     A scenario encapsulating <see cref="Data"/>, with the transformed description.
    /// </returns>
    [Pure]
    public Scenario<TData> WithTransformedDescription(Func<string, string> transformation)
        => WithDescription(transformation(Description));

    /// <summary>
    /// Transforms the scenario's description (or <see cref="string.Empty"/>, if none has been set) by
    /// appending <paramref name="appendage"/> to it.
    /// </summary>
    /// <param name="appendage">The string to append to the description.</param>
    /// <returns>
    ///     A scenario encapsulating <see cref="Data"/>, with the appended description.
    /// </returns>
    [Pure]
    public Scenario<TData> WithDescriptionAppendage(string appendage)
        => WithTransformedDescription(description => $"{description} {appendage}");

    /// <summary>
    /// Transforms the scenario's data by applying <paramref name="transformation"/> to it.
    /// </summary>
    /// <param name="transformation">The transformation to apply to the <see cref="Data"/>.</param>
    /// <typeparam name="TTransformedData">The type of the transformed data.</typeparam>
    /// <returns>
    ///     A scenario the transformed data, with the previously provided description (if existent).
    /// </returns>
    [Pure]
    public Scenario<TTransformedData> WithTransformedData<TTransformedData>(
        Func<TData, TTransformedData> transformation) => new(transformation(Data));

    /// <summary>
    /// Transforms the scenario by combining it with <paramref name="otherScenario"/>, applying
    /// <paramref name="resultDataSelector"/> and <paramref name="resultDescriptionSelector"/> to obtain result data and
    /// description, respectively.
    /// </summary>
    /// <param name="otherScenario">The scenario to combine this scenario with.</param>
    /// <param name="resultDataSelector">
    ///     The function used to obtain the data for the result scenario by using this and the other scenario's data as
    ///     inputs.
    /// </param>
    /// <param name="resultDescriptionSelector">
    ///     The function used to obtain the description for the result scenario by using this and the other scenario's
    ///     descriptions as inputs.
    /// </param>
    /// <typeparam name="TOtherData">The type of the other scenario's data.</typeparam>
    /// <typeparam name="TCombinedData">The type of the result scenario's data.</typeparam>
    /// <returns>A scenario representing the result of combining this and the other scenario.</returns>
    [Pure]
    public Scenario<TCombinedData> CombinedWith<TOtherData, TCombinedData>(
        Scenario<TOtherData> otherScenario,
        Func<TData, TOtherData, TCombinedData> resultDataSelector,
        Func<string, string, string> resultDescriptionSelector)
        => WithTransformedData(data => resultDataSelector(data, otherScenario.Data))
            .WithTransformedDescription(
                description => resultDescriptionSelector(description, otherScenario.Description));
    
    /// <summary>
    /// Transforms the scenario by combining it with <paramref name="otherScenario"/>, pairing up data an descriptions.
    /// </summary>
    /// <param name="otherScenario">The scenario to combine this scenario with.</param>
    /// <typeparam name="TOtherData">The type of the other scenario's data.</typeparam>
    /// <returns>A scenario consisting of the paired up data and descriptions.</returns>
    [Pure]
    public Scenario<(TData First, TOtherData Second)> CombinedWith<TOtherData>(
        Scenario<TOtherData> otherScenario)
        => CombinedWith(
            otherScenario,
            (data, otherData) => (data, otherData),
            (description, otherDescription) => $"({description}, {otherDescription})");

    /// <summary>
    /// Unwraps the <paramref name="scenario"/> by implicitly converting to <typeparamref name="TData"/>
    /// and returning the underlying <see cref="Data"/>.
    /// </summary>
    /// <param name="scenario">The scenario to unwrap.</param>
    /// <returns>The underlying <see cref="Data"/>.</returns>
    [Pure]
    public static implicit operator TData(Scenario<TData> scenario) => scenario.Data;
}
