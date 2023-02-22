using JetBrains.Annotations;

namespace Relaxdays.TestUtilities.Scenarios;

/// <summary>
///     Provides static methods for use with <see cref="Scenario{TData}" />.
/// </summary>
[PublicAPI]
public static class Scenario
{
    /// <summary>
    ///     Transforms the scenarios by pairing up data and descriptions, using
    ///     <see cref="Scenario{TData}.CombinedWith{TOtherData}(Scenario{TOtherData})" />.
    /// </summary>
    /// <remarks>
    ///     One use case is to provide this as a method group in a selector function, e.g.
    ///     <br />
    ///     <code>
    ///         var playersAndStages = players.Zip(stages, Scenario.Combine);
    ///     </code>
    /// </remarks>
    /// <param name="first">The first scenario to pair up.</param>
    /// <param name="second">The second scenario to pair up.</param>
    /// <typeparam name="TFirstData">The type of the first scenario's data.</typeparam>
    /// <typeparam name="TSecondData">The type of the second scenario's data.</typeparam>
    /// <returns>A scenario consisting of the paired up data and descriptions.</returns>
    [Pure]
    public static Scenario<(TFirstData First, TSecondData Second)> Combine<TFirstData, TSecondData>(
        Scenario<TFirstData> first, Scenario<TSecondData> second) => first.CombinedWith(second);
}
