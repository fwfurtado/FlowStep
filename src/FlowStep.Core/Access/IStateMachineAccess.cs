using FlowStep.Core.Support;

namespace FlowStep.Core.Access;

public interface IStateMachineAccess<TState, TEvent>
    where TState : notnull
{
    IStateMachine<TState, TEvent> Relay { get; }
    IStateMachine<TState, TEvent>? Parent { get; }
    IMessage<TEvent> ForwardedInitialEvent { get; }

    bool? InitialEnabled { get; }

    void AddStateMachineInterceptor(IStateMachineInterceptor<TState, TEvent> interceptor);
}