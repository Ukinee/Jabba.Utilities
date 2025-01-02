namespace Jabba.Infrastructure.Abstractions;

public interface IFactory<out T>
{
    public T Create();
}

public interface IFactory<out TResult, in TPayload>
{
    public TResult Create(TPayload payload);
}
