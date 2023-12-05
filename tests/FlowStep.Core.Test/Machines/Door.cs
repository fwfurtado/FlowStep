namespace FlowStep.Core.Test.Machines;

public enum DoorState
{
    Opened,
    Closed,
    Locked
}

public enum DoorEvent
{
    Open,
    Close,
    Lock,
    Unlock
}

public class DoorStateMachineFactory
{
    // public StateMachine<DoorState, DoorEvent> Configure()
    // {
    //     var factory = new StateMachineFactory<DoorState, DoorEvent>();
    //
    //     return factory.Configure(configure =>
    //     {
    //         configure.State(DoorState.Closed, state =>
    //         {
    //             state.AddTransition(DoorEvent.Open, DoorState.Opened);
    //             state.AddTransition(DoorEvent.Lock, DoorState.Locked);
    //         });
    //
    //         configure.State(DoorState.Opened, state =>
    //         {
    //             state.AddTransition(DoorEvent.Close, DoorState.Closed);
    //         });
    //         
    //         configure.State(DoorState.Locked, state =>
    //         {
    //             state.AddTransition(DoorEvent.Unlock, DoorState.Closed);
    //         });
    //     });
    // }
}