namespace FlowStep.Core.Support;

public interface IMessage<out T>
{
    T Payload { get; }
    MessageHeaders Headers { get; }
}

public class MessageHeaders : Dictionary<string, object>;