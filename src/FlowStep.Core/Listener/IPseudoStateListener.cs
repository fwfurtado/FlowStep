using FlowStep.Core.State.Pseudo;

namespace FlowStep.Core.Listener;

public interface IPseudoStateListener<TState, TEvent>
{
    void OnContext(IPseudoStateContext<TState, TEvent> context);
}