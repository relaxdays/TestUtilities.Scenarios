using JetBrains.Annotations;

// CS1573: Parameter 'parameter' has no matching param tag in the XML comment
// Justification: False positive, since we inherit the description for data (in various variants of the extension)
#pragma warning disable CS1573

namespace Relaxdays.TestUtilities.Scenarios;

/// <summary>
///     Extensions for any type.
/// </summary>
[PublicAPI]
public static class GenericExtensions
{
    /// <summary>
    ///     Wraps <paramref name="data" /> in a <see cref="Scenario{TData}" /> with an empty description.
    /// </summary>
    /// <param name="data">The data to wrap.</param>
    /// <typeparam name="TData">The type of the data to wrap.</typeparam>
    /// <returns>The data wrapped in a scenario.</returns>
    [Pure]
    public static Scenario<TData> AsScenario<TData>(this TData data) => new(data);

    /// <summary>
    ///     Wraps <paramref name="data" /> in a <see cref="Scenario{TData}" /> with the provided
    ///     <paramref name="description" />.
    /// </summary>
    /// <inheritdoc cref="AsScenario{TData}(TData)" />
    /// <param name="description">The description for the scenario.</param>
    [Pure]
    public static Scenario<TData> AsScenario<TData>(this TData data, string description)
        => data.AsScenario().WithDescription(description);

    /// <summary>
    ///     Wraps <see cref="data" /> in a <see cref="Scenario{TData}" /> with a description created by applying
    ///     <paramref name="descriptionSelector" /> to <paramref name="data" />.
    /// </summary>
    /// <remarks>
    ///     One use case is when <c>new</c>'ing data that is then promptly transformed into a scenario with a
    ///     description, to have access to the data value without having to save it in a temporary variable, e.g.:
    ///     <br />
    ///     <code>
    ///         var scenario = new Player("T. Hawk").AsScenario(player => player.Name);
    ///     </code>
    /// </remarks>
    /// <inheritdoc cref="AsScenario{TData}(TData)" />
    /// <param name="descriptionSelector">The function used to construct the description from the data.</param>
    [Pure]
    public static Scenario<TData> AsScenario<TData>(this TData data, Func<TData, string> descriptionSelector)
        => data.AsScenario(descriptionSelector(data));
}
