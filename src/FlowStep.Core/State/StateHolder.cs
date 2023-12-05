namespace FlowStep.Core.State;

public class StateHolder<TState, TEvent>
{
    public required IState<TState, TEvent> State { get; set; }
}