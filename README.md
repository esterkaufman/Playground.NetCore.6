# Playground.NetCore.6
Playground repo for all gists and projects that i play with to implement programming concepts

# Async State Machine in .NET Core
This is an implementation of an async state machine in .NET Core C# 6.0, with no dependencies on external libraries. The state machine is designed to support generically-typed states and to allow for loading of configuration from external classes or from JSON files.

## Features
* Asynchronous execution of state transitions
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
