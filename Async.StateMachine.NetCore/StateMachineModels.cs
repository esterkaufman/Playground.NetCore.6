public class StateMachineConfig<TState, TEvent>
{
    public TState InitialState { get; set; }
    public List<State<TState, TEvent>> StateDefinitions { get; set; }
    public ActionConfig<TState>[]? EntryActions { get; set; }
    public ActionConfig<TState>[]? ExitActions { get; set; }
}

public class State<TState, TEvent>
{
    public TState Key { get; set; }
    public List<Transition<TState, TEvent>> Transitions { get; set; }
}

public class Transition<TState, TEvent>
{
    public TEvent Trigger { get; set; }
    public TState NextState { get; set; }
}
public class ActionConfig<TState>
{
    public TState State { get; set; }
    public Func<Task> Action { get; set; }
}