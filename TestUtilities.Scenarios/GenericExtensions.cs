using JetBrains.Annotations;

namespace Relaxdays.TestUtilities.Scenarios;

/// <summary>
/// Extensions for any type.
/// </summary>
[PublicAPI]
public static class GenericExtensions
{
    /// <summary>
    /// Wraps <see cref="data"/> in a <see cref="Scenario{TData}"/> with an empty description.
    /// </summary>
    /// <param name="data">The data to wrap.</param>
    /// <typeparam name="TData">The type of the data to wrap.</typeparam>
    /// <returns>The data wrapped in a scenario.</returns>
    [Pure]
    public static Scenario<TData> AsScenario<TData>(this TData data) => new(data);

    /// <summary>
    /// Wraps <see cref="data"/> in a <see cref="Scenario{TData}"/> with the provided
    /// <paramref name="description"/>.
    /// </summary>
    /// <inheritdoc cref="AsScenario{TData}(TData)"/>
    /// <param name="description">The description for the scenario.</param>
    [Pure]
    public static Scenario<TData> AsScenario<TData>(
        // CS1573: Parameter 'parameter' has no matching param tag in the XML comment
        // Justification: False positive, since we inherit the description for data
#pragma warning disable CS1573
        this TData data,
#pragma warning restore CS1573
        string description)
        => data.AsScenario().WithDescription(description);

    // TODO: Comments
    // TODO: Disable CS1573 for the file / class / via SuppressMessage
    [Pure]
    public static Scenario<TData> AsScenario<TData>(this TData data, Func<TData, string> descriptionSelector)
        => data.AsScenario(descriptionSelector(data));
}
