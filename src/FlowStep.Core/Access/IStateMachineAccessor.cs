namespace FlowStep.Core.Access;

public interface IStateMachineAccessor<TState, TEvent>
    where TState : notnull
{
    void DoWithAllRegions(Action<IStateMachineAccess<TState, TEvent>> action);
    void DoAllRegions(Action<IStateMachineAccess<TState, TEvent>> action);

    IEnumerable<IStateMachineAccess<TState, TEvent>> AllRegions { get; }
    IStateMachineAccess<TState, TEvent> Region { get; }
}