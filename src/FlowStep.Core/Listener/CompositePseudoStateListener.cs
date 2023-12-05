using FlowStep.Core.State.Pseudo;

namespace FlowStep.Core.Listener;

public class CompositePseudoStateListener<TState, TEvent> :
    AbstractCompositeListener<IPseudoStateListener<TState, TEvent>>, IPseudoStateListener<TState, TEvent>
{
    public void OnContext(IPseudoStateContext<TState, TEvent> context)
    {
        foreach (var listener in Listeners.InReverseOrder)
        {
            listener.OnContext(context);
        }
    }
}