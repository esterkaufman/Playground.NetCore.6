public record StateMachineConfig<TState, TEvent>
(
   TState InitialState,
   List<State<TState, TEvent>> StateDefinitions,
   ActionConfig<TState>[]? EntryActions,
   ActionConfig<TState>[]? ExitActions
);
public record State<TState, TEvent>(TState Key, List<Transition<TState, TEvent>> Transitions);
public record Transition<TState, TEvent>(TEvent Trigger, TState NextState);
public record ActionConfig<TState>(TState State,Func<Task> Action);
