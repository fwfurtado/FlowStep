using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.State.Extended;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Default;

public class DefaultStateContext<TState, TEvent>
(
    Stage stage,
    IStateMachine<TState, TEvent> machine,
    IMessage<TEvent>? message = null,
    MessageHeaders? headers = null,
    ITransition<TState, TEvent>? transition = null,
    IExtendedState? extendedState = null,
    IState<TState, TEvent>? source = null,
    IState<TState, TEvent>? target = null,
    IEnumerable<IState<TState, TEvent>>? sources = null,
    IEnumerable<IState<TState, TEvent>>? targets = null,
    Exception? exception = null
) : IStateContext<TState, TEvent>
{
    public Stage Stage { get; } = stage;
    public IMessage<TEvent>? Message { get; } = message;
    public TEvent? Event { get; } = message != null ? message.Payload : default;
    public MessageHeaders? Headers { get; } = headers;

    public IExtendedState? ExtendedState { get; } = extendedState;
    public ITransition<TState, TEvent>? Transition { get; } = transition;
    public IStateMachine<TState, TEvent> StateMachine { get; } = machine;

    public IState<TState, TEvent>? Source { get; } = source ?? transition?.Source;
    public IEnumerable<IState<TState, TEvent>>? Sources { get; } = sources;

    public IState<TState, TEvent>? Target { get; } = target ?? transition?.Target;
    public IEnumerable<IState<TState, TEvent>>? Targets { get; } = targets;

    public Exception? Error { get; } = exception;

    public object? GetHeader(string key)
    {
        if (Headers is null) return null;

        var found = Headers.TryGetValue(key, out var value);

        return found ? value : null;
    }
}