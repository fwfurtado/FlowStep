using FlowStep.Core.Context;

namespace FlowStep.Core.Listener;

public interface IStateListener<TState, TEvent>
{
    void OnEntry(IStateContext<TState, TEvent> context);
    void OnExit(IStateContext<TState, TEvent> context);
    void OnComplete(IStateContext<TState, TEvent> context);
    
    void DoOnComplete(IStateContext<TState, TEvent> context);
}