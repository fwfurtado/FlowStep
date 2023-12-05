using FlowStep.Core.Action;
using FlowStep.Core.Support;

namespace FlowStep.Core.Listener;

public class CompositeActionListener<TState, TEvent>
    : AbstractCompositeListener<IActionListener<TState, TEvent>>, IActionListener<TState, TEvent>
{
    public void OnExecute(
        IStateMachine<TState, TEvent> stateMachine,
        StateContextAction<TState, TEvent> action,
        long duration
    )
    {
        foreach (var listener in Listeners.InReverseOrder)
        {
            listener.OnExecute(stateMachine, action, duration);
        }
    }
}