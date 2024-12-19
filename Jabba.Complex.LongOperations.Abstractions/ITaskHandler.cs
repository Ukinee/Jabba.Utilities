namespace Jabba.Complex.LongOperations.Abstractions;

public interface ITaskHandler
{
    public string Stage { get; }
    
    public void StartStage(string stage);
    public void StopStage();
}
