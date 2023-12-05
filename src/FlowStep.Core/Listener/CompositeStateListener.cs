using FlowStep.Core.Context;

namespace FlowStep.Core.Listener;

public class CompositeStateListener<TState, TEvent> : AbstractCompositeListener<IStateListener<TState, TEvent>>,
    IStateListener<TState, TEvent>
{
    public void OnEntry(IStateContext<TState, TEvent> context)
    {
        Notify(listener => listener.OnEntry(context));
    }

    public void OnExit(IStateContext<TState, TEvent> context)
    {
        Notify(listener => listener.OnExit(context));
    }

    public void OnComplete(IStateContext<TState, TEvent> context)
    {
        Notify(listener => listener.OnComplete(context));
    }

    public void DoOnComplete(IStateContext<TState, TEvent> context)
    {
        Notify(listener => listener.DoOnComplete(context));
    }

    private void Notify(Action<IStateListener<TState, TEvent>> action)
    {
        foreach (var listener in Listeners.InReverseOrder)
        {
            action(listener);
        }
    }
}