namespace Jabba.Complex.LongOperations
{
    public enum LongOperationState
    {
        WaitingForStart,
        Starting,
        Running,
        Completed,
        StoppingSoftly,
        StoppingImmediately,
        Stopped,
        Failed,
        Disposed,
    }
}
