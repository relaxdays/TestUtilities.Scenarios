# Examples

## Overview

These example projects showcase `Scenario` usage with different test frameworks and provide a comparison to how output
looks when the mechanism is not used.

## Usage

The `showcase` script can be used to comfortably compare results of running the tests with and without scenarios. It
expects a parameter which can either be `raw` or `scenario`.

## Sample outputs

ℹ A more detailed discussion of where the `Scenario` can be useful for the different test frameworks, and what
alternatives there are, is going to be added (and referenced from here) in a future update.

### Raw test case parameters

This is a sample output of running the `showcase` script with parameter `raw`:

```console
$ ./showcase raw
  Failed MSTest_Player_names_should_get_capitalized_raw (Examples.Common.Player) [16 ms]
  Failed MSTest_Player_names_should_get_capitalized_raw (Examples.Common.Player) [1 ms]
  Failed NUnit_Player_names_should_get_capitalized_raw(Examples.Common.Player) [54 ms]
  Failed NUnit_Player_names_should_get_capitalized_raw(Examples.Common.Player) [1 ms]
  Failed Examples.XUnit.SampleTests.XUnit_Player_names_should_get_capitalized_raw(player: Player { Name = "rose" }) [4 ms]
  Failed Examples.XUnit.SampleTests.XUnit_Player_names_should_get_capitalized_raw(player: Player { Name = "fei long" }) [< 1 ms]
```

We can see how for `MSTest` and `NUnit`, it is not easily discernible what concrete case failed for a test. `XUnit` does
better, as it outputs the parameters in a similar way to how a `record` is printed.

### Scenario test case parameters

This is a sample output of running the `showcase` script with parameter `scenario`:

```console
$ ./showcase scenario
  Failed MSTest_Player_names_should_get_capitalized_scenario ("e.honda") [13 ms]
  Failed MSTest_Player_names_should_get_capitalized_scenario ("chun-li") [1 ms]
  Failed NUnit_Player_names_should_get_capitalized_scenario("cammy") [35 ms]
  Failed NUnit_Player_names_should_get_capitalized_scenario("dhalsim") [1 ms]
  Failed Examples.XUnit.SampleTests.XUnit_Player_names_should_get_capitalized_scenario(playerScenario: "dee jay") [2 ms]
  Failed Examples.XUnit.SampleTests.XUnit_Player_names_should_get_capitalized_scenario(playerScenario: "blanka") [< 1 ms]
```

We can see that for each test framework it is now possible to find out which concrete case failed by way of the provided
descriptions. `XUnit` does not look much different from the `raw` case in this small example, we could however imagine
that we have a more complex input object, for which a concise `Scenario`-descriptions is more helpful to see what's
going on as opposed to the auto-generated display string.
