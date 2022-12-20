using System.Text.Json;

public class AsyncStateMachine<TState, TEvent> where TState : notnull
{
    private readonly Dictionary<TState, Dictionary<TEvent, TState>> _transitions = new();
    private readonly Dictionary<TState, Func<Task>> _entryActions = new();
    private readonly Dictionary<TState, Func<Task>> _exitActions = new();

    public TState CurrentState { get; private set; }

    public AsyncStateMachine(StateMachineConfig<TState, TEvent> config)
    {
        Setup(config);
    }
    public async Task LoadConfigFromFile(string path)
    {

        var config = JsonSerializer.Deserialize<StateMachineConfig<TState, TEvent>>(await File.ReadAllTextAsync(path), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        Setup(config);
    }

    public void Setup(StateMachineConfig<TState, TEvent> config)
    {
        if (config == null)
            throw new Exception("Config cannot be null when creating StateMachine");
        CurrentState = config.InitialState;

        foreach (var state in config.StateDefinitions)
        {
            AddTransition(state.Key, state.Transitions);
        }

        if (config.EntryActions != null)
            foreach (var entryAction in config.EntryActions)
            {
                AddEntryAction(entryAction.State, entryAction.Action);
            }

        if (config.ExitActions != null)
            foreach (var exitAction in config.ExitActions)
            {
                AddExitAction(exitAction.State, exitAction.Action);
            }
    }

    public async Task<TState>  TriggerAsync(TEvent trigger)
    {
        if (!_transitions.TryGetValue(CurrentState, out var triggers)
            || !triggers.TryGetValue(trigger, out var nexState))
        {
            throw new Exception($"'{trigger}' Event is not supported!");
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

    void AddTransition(TState state, IReadOnlyCollection<Transition<TState, TEvent>>? stateTriggers)
    {
        if (stateTriggers != null)
            _transitions.Add(state, stateTriggers.ToDictionary(entry => entry.Trigger, entry => entry.NextState));
    }


    void AddEntryAction(TState state, Func<Task> action)
    {
        _entryActions.Add(state, action);
    }

    void AddExitAction(TState state, Func<Task> action)
    {
        _exitActions.Add(state, action);
    }

    
}

