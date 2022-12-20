var stateMachine = new AsyncStateMachine<string, string>();

await stateMachine.LoadConfigFromFile("stateMap.json");

PrintState(stateMachine.CurrentState);
PrintState(await stateMachine.TriggerAsync("Processing"));
PrintState(await stateMachine.TriggerAsync("Success"));
PrintState(await stateMachine.TriggerAsync("Finish"));
PrintState(await stateMachine.TriggerAsync("Failure")); //Throw as the event is not allowed on Ready state

Console.Read();

void PrintState(string state)
{
    Console.WriteLine($"Machine Current State: {state}");
}


/*
 ----------------------OUTPUT-------------------------------:
Machine Current State: Ready
Machine Current State: Process
Machine Current State: Result
Machine Current State: Ready
Unhandled exception. System.Exception: 'Failure' Event is not supported on current state: 'Ready'.
*/