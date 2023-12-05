using FlowStep.Core.Action;
using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;

namespace FlowStep.Core.State;

public interface IState<TState, TEvent>
{
    TState Id { get; }
    IEnumerable<TState> Ids { get; }
    IEnumerable<IState<TState, TEvent>> States { get; }
    IPseudoState<TState, TEvent>? Pseudo { get; }

    IEnumerable<StateContextAction<TState, TEvent>> EntryActions { get; }
    IEnumerable<StateContextAction<TState, TEvent>> StateActions { get; }
    IEnumerable<StateContextAction<TState, TEvent>> ExitActions { get; }

    bool IsSimple { get; }
    bool IsComposite { get; }
    bool IsOrthogonal { get; }
    bool IsSubMachineState { get; }


    IEnumerable<IStateMachineEventResult<TState, TEvent>> Send(IMessage<TEvent> @event);

    void Entry(IStateContext<TState, TEvent> context);
    void Exit(IStateContext<TState, TEvent> context);

    void AddStateListener(IStateListener<TState, TEvent> listener);
    void RemoveStateListener(IStateListener<TState, TEvent> listener);

    void AddActionListener(IActionListener<TState, TEvent> listener);
    void RemoveActionListener(IActionListener<TState, TEvent> listener);
}