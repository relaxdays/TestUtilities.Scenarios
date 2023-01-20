using JetBrains.Annotations;

namespace Relaxdays.TestUtilities.Scenarios;

// This record is internal because it is not intended for outside use and exists only to implement the
// "custom description logic". For everything to work as intended, users should always use base type Scenario<TData>
internal record ScenarioWithDescription<TData>(TData Data, string? Description = null) : Scenario<TData>(Data)
{
    /// <inheritdoc />
    [Pure]
    public override string ToString() => Description is not null
        ? $"\"{Description}\""
        : base.ToString();

    /// <inheritdoc />
    [Pure]
    public override Scenario<TData> WithTransformedDescription(Func<string, string> transformation)
        => Description is not null
            ? new ScenarioWithDescription<TData>(Data, transformation(Description))
            : base.WithTransformedDescription(transformation);

    /// <inheritdoc />
    [Pure]
    public override Scenario<TTransformedData> WithTransformedData<TTransformedData>(
        Func<TData, TTransformedData> transformation)
    {
        var transformedBaseScenario = base.WithTransformedData(transformation);

        return Description is not null
            ? transformedBaseScenario.WithDescription(Description)
            : transformedBaseScenario;
    }
}