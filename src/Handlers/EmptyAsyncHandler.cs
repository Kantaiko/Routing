namespace Kantaiko.Routing.Handlers;

public class EmptyAsyncHandler<TInput> : IHandler<TInput, Task<Unit>>
{
    public Task<Unit> Handle(TInput input) => Unit.Task;

    public static EmptyAsyncHandler<TInput> Instance { get; } = new();
}
