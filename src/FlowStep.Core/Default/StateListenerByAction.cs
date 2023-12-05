using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.Support;

namespace FlowStep.Core.Default;

public class StateListenerByAction<TState, TEvent>
(
    StateContextAction<TState, TEvent>? onEntry = null,
    StateContextAction<TState, TEvent>? onExit = null,
    StateContextAction<TState, TEvent>? onComplete = null,
    StateContextAction<TState, TEvent>? doOnComplete = null
) : IStateListener<TState, TEvent>
{
    public void OnEntry(IStateContext<TState, TEvent> context)
    {
        onEntry?.Invoke(context);
    }

    public void OnExit(IStateContext<TState, TEvent> context)
    {
        onExit?.Invoke(context);
    }

    public void OnComplete(IStateContext<TState, TEvent> context)
    {
        onComplete?.Invoke(context);
    }

    public void DoOnComplete(IStateContext<TState, TEvent> context)
    {
        doOnComplete?.Invoke(context);
    }
}