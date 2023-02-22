# Relaxdays.Scenarios.TestUtilities

## Overview

This project is focused around a wrapper type designated to produce clearer unit test output by letting users provide a
description for test methods parameters. It works with [MSTest](Examples/Examples.MSTest/),
[NUnit](Examples/Examples.NUnit/) and [XUnit](Examples/Examples.XUnit/). While these test frameworks also provide ways
to achieve more readable unit test output, the `Scenario` type introduced in this project has some extra features.

_ℹ A discussion of `Scenario` pros and cons and comparison with the builtin mechanisms will be added at a later point._

## Usage

### Basic

Creating a scenario with a description is simple:

```csharp
// Create a scenario wrapping a player and set it's description to the player's name
var playerScenario = new Player("Ryu").AsScenario(player => player.Name);

// Create a scenario wrapping a preset bonus stage and give it a description that is meaningful in the context of our
// test (i.e., the stage is only for one player)
var stageScenario = BonusStages.DestroyCar.AsScenario("Single player stage (Bonus Stage: Destroy Car)");
```

We can use scenarios created this way as inputs to our unit tests:

```csharp
[TestCaseSource(typeof(Stages), nameof(Stages.All))]
public void A_match_has_three_rounds(Scenario<Stage> stage)
{
    // ...
}
```

If we then run our unit tests and one fails, we get useful information from the descriptions provided, which might help
us diagonse what's causing the issue more easily:

```console
Failed A_match_has_three_rounds ("Single player stage (Bonus Stage: Destroy Car)")
```

Here we can see at a glance that our test failed because the input stage is actually a bonus stage, which does not have
three rounds.

### Advanced

Various methods are provided for fluent (non-mutating) transformation and combination of scenarios:

```csharp
// Transform a scenario's data (halfing health of an existing player scenario) and adjust description accordingly
var playerWithHalfHealthScenario = playerScenario
    .WithTransformedData(player => player with { Health = player.Health / 2 })
    // We could also use the more specific .WithDescriptionAppendage("with half health") here
    .WithTransformedDescription(description => $"{description} with half health")

// Combine two scenarios by pairing them up, more generally, result data and description selectors can also be provided
var playerAndStage = playerScenario.CombinedWith(stageScenario);
```
