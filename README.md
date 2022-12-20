# Playground.NetCore.6
Playground repo for all gists and projects that i play with to implement programming concepts

# Async State Machine in .NET Core
This is an implementation of an async state machine in .NET Core C# 6.0, with no dependencies on external libraries. The state machine is designed to support generically-typed states and to allow for loading of configuration from external classes or from JSON files.

## Features
* Asynchronous execution of state transition actions (Entry / Exit actions)
* Support for generically-typed states
* Ability to load configuration from external classes or from JSON files

## Getting Started
To use the state machine, you will need to install .NET Core 6.0 or higher on your system.

You can then clone this repository and build the solution. The state machine is implemented in the `AsyncStateMachine` class, which can be found in the `Async.StateMachine.NetCore` project.

To create a new state machine, you will need to define your states and transitions. 

For example:

```c#
public static StateMachineConfig<string,string> StateConfig = new StateMachineConfig<string,string>("Ready",
        new List<State<string,string>>
        {
            new ("Ready", new List<Transition<string, string>>
                {
                    new ("Processing",  "Process")
                }
            ),
            new ("Process", new List<Transition<string, string>>
                {
                    new ("Success",  "Result"),
                    new ("Failure",  "Error"),
                }
            ),
            new ("Error", new List<Transition<string, string>>
                {
                    new ("Retry",  "Process")
                }
            ),
            new ("Result", new List<Transition<string, string>>
                {
                    new ("Finish",  "Ready")
                }
            )},default,default);
```

You can then create a new instance of the `AsyncStateMachine` class: 

`var stateMachine = new AsyncStateMachine<string, string>();`


To trigger a transition, you can call the  `TriggerAsync` method and pass it the trigger name and any additional arguments required by the transition:

`await stateMachine.TriggerAsync("Processing");`

This method will make the transien of the state to the next state pre configures in `Setup`.
And, most important, will find and execute the ExitAction on the current state before the move, and the EntryAction on the next state.

### Source code of `TriggerAsync` :

```csharp
public async Task<TState>  TriggerAsync(TEvent trigger)
    {
        if (!_transitions.TryGetValue(CurrentState, out var triggers)
            || !triggers.TryGetValue(trigger, out var nexState))
        {
            throw new Exception($"'{trigger}' Event is not supported on current state: '{CurrentState}'.");
        }

        // Invoke exit on current state
        if (_exitActions.ContainsKey(CurrentState))
            await _exitActions[CurrentState]();
        // Transient state
        CurrentState = nexState;
        // Invoke entry on udated state
        if (_entryActions.ContainsKey(CurrentState))
            await _entryActions[CurrentState]();
        return CurrentState;
    }
```

### You can also load the configuration for the state machine from an external class or from a JSON file. 

* To load from an external class, you can pass an instance of the states list to the `Setup` method:

`stateMachine.Setup(states)`
 
* To load from a JSON file, you can use the `LoadConfigFromFile` method and pass it the path to the JSON file:

` await stateMachine.LoadConfigFromFile("stateMap.json");`

Where json content is like:

```json
{
  "initialState":"Ready",
  "stateDefinitions":[
    {
      "key":"Ready",
      "transitions":[{"trigger":"Processing","nextState":"Process"}]
    },
    {
      "key":"Process",
      "transitions":[
        {"trigger":"Success","nextState":"Result"},
        {"trigger":"Failure","nextState":"Error"}]},
    {
      "key":"Error",
      "transitions":[{"trigger":"Retry","nextState":"Process"}]},
    {
      "key":"Result",
      "transitions":[{"trigger":"Finish","nextState":"Ready"}]
    }]
}
```

## Examples

For more detailed examples of how to use the async state machine, see the `StateMap_Example` class.
