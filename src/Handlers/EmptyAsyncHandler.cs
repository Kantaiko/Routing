namespace Kantaiko.Routing.Handlers;

internal class EmptyAsyncHandler<TInput> : IHandler<TInput, Task>, IChainedHandler<TInput, Task>
{
    public static EmptyAsyncHandler<TInput> Instance { get; } = new();

    public Task Handle(TInput input)
    {
        return Task.CompletedTask;
    }

    public Task Handle(TInput input, Func<TInput, Task> next)
    {
        return Task.CompletedTask;
    }
}
