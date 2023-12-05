using FlowStep.Core.Support;

namespace FlowStep.Core.Action;

public interface IActionListener<TState, TEvent>
{
    void OnExecute(
        IStateMachine<TState, TEvent> stateMachine,
        StateContextAction<TState, TEvent> action,
        long duration
    );
}