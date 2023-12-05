using FlowStep.Core.Context;

namespace FlowStep.Core.Support;

public delegate bool Guard<TState, TEvent>(IStateContext<TState, TEvent> context);

public delegate void StateContextAction<TState, TEvent>(IStateContext<TState, TEvent> context);

public delegate void ExtendedStateChanged(object key, object value);