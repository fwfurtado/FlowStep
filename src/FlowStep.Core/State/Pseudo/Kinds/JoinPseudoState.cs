using FlowStep.Core.Context;
using FlowStep.Core.Default;
using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class JoinPseudoState<TState, TEvent> : AbstractPseudoState<TState, TEvent>
{
    private readonly JoinTracker _tracker;
    private readonly IEnumerable<PseudoStateData<TState, TEvent>> _targets;

    public JoinPseudoState(
        List<List<IState<TState, TEvent>>> joins,
        IEnumerable<PseudoStateData<TState, TEvent>> targets
    )
    {
        _targets = targets;

        _tracker = new JoinTracker(joins, () => Notify(new DefaultPseudoStateContext<TState, TEvent>(this)));
    }

    public override PseudoStateKind Kind => PseudoStateKind.Join;

    public override IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        foreach (var target in _targets)
        {
            if (target.Guard(context))
            {
                return target.State;
            }
        }

        return null;
    }

    public override void Exit(IStateContext<TState, TEvent> context)
    {
        _tracker.Reset();
    }

    public void Reset(ICollection<TState> ids)
    {
        _tracker.Reset(ids);
    }

    private class JoinCompleter(System.Action onComplete)
    {
        private bool _isComplete;

        public bool IsIncomplete => !_isComplete;

        public void Complete()
        {
            _isComplete = true;
            onComplete();
        }

        public void Reset()
        {
            _isComplete = false;
        }
    }

    private class JoinTracker
    {
        private readonly JoinCompleter _joinCompleter;
        private readonly List<List<IState<TState, TEvent>>> _track;
        private readonly List<List<IState<TState, TEvent>>> _allStates;


        public JoinTracker(
            List<List<IState<TState, TEvent>>> allStates,
            System.Action action
        )
        {
            _joinCompleter = new JoinCompleter(action);

            _allStates = allStates;

            _track = new List<List<IState<TState, TEvent>>>(allStates.Count);

            foreach (var states in allStates)
            {
                _track.Add(new List<IState<TState, TEvent>>(states));

                foreach (var state in states)
                {
                    var listener = new JoinTrackActionListener(_track, state, _joinCompleter);
                    state.AddStateListener(listener);
                }
            }
        }


        public void Reset()
        {
            _track.Clear();

            foreach (var states in _allStates)
            {
                _allStates.Add(new List<IState<TState, TEvent>>(states));
            }

            _joinCompleter.Reset();
        }

        public void Reset(ICollection<TState> ids)
        {
            Reset();

            foreach (var track in _track)
            {
                foreach (var state in track)
                {
                    if (ids.Contains(state.Id))
                    {
                        track.Remove(state);
                    }
                }
            }
        }
    }

    private class JoinTrackActionListener
    (
        List<List<IState<TState, TEvent>>> track,
        IState<TState, TEvent> state,
        JoinCompleter completer
    ) : IStateListener<TState, TEvent>
    {
        public void OnEntry(IStateContext<TState, TEvent> context)
        {
            //NO-OP
        }

        public void OnExit(IStateContext<TState, TEvent> context)
        {
            //NO-OP
        }

        public void OnComplete(IStateContext<TState, TEvent> context)
        {
            lock (track)
            {
                foreach (var states in track)
                {
                    states.Remove(state);
                }

                if (completer.IsIncomplete && track.Count == 0)
                {
                    completer.Complete();
                }
            }
        }

        public void DoOnComplete(IStateContext<TState, TEvent> context)
        {
            //NO-OP
        }
    }
}