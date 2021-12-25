namespace Kantaiko.Routing.Handlers;

public class EmptyAsyncHandler<TInput> : IChainedHandler<TInput, Task<Unit>>
{
    public Task<Unit> Handle(TInput input, Func<TInput, Task<Unit>> next) => Unit.Task;

    public Task<Unit> Handle(TInput input) => Unit.Task;

    public static EmptyAsyncHandler<TInput> Instance { get; } = new();
}
