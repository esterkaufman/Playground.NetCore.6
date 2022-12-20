using System.Text.Json;

var statesConfig = JsonSerializer.Deserialize<StateMachineConfig<string, string>>(await File.ReadAllTextAsync("stateMap.json"), new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
});
if (statesConfig != null)
{
    var stateMachine = new AsyncStateMachine<string, string>(statesConfig);

    PrintState(stateMachine.CurrentState);
    PrintState(await stateMachine.ProcessEvent("Processing"));
    PrintState(await stateMachine.ProcessEvent("Success"));
    PrintState(await stateMachine.ProcessEvent("Finish"));
    PrintState(await stateMachine.ProcessEvent("Failure")); //Throw as the event is not allowed on Ready state
}

Console.Read();

void PrintState(string state)
{
    Console.WriteLine($"Machine Current State: {state}");
}
//----------------------OUTPUT-------------------------------
/*
Machine Current State: Ready
Machine Current State: Process
Machine Current State: Result
Machine Current State: Ready
Unhandled exception. System.Exception: 'Failure' Event is not supported!
 */