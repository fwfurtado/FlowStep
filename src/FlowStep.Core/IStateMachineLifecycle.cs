namespace FlowStep.Core;

public interface IStateMachineLifecycle
{
    bool IsRunning { get; }
    
    void Start();
    void Stop();
}