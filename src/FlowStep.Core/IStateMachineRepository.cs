using FlowStep.Core.Context;

namespace FlowStep.Core;

public interface IStateMachineRepository<TState, TEvent, TContext>
    where TContext : IStateMachineContext<TState, TEvent>
    where TState : notnull
{
    void Save(TContext context, string id);

    TContext? Load(string id);
}