namespace Kantaiko.Routing.Handlers;

internal class EmptyHandler<TInput> :
    IHandler<TInput>,
    IHandler<TInput, Task>,
    IHandler<TInput, Task<Unit>>,
    IChainedHandler<TInput>,
    IChainedHandler<TInput, Task>,
    IChainedHandler<TInput, Task<Unit>>
{
    public static EmptyHandler<TInput> Instance { get; } = new();

    void IHandler<TInput>.Handle(TInput input) { }

    Task IHandler<TInput, Task>.Handle(TInput input)
    {
        return Task.CompletedTask;
    }

    Task<Unit> IHandler<TInput, Task<Unit>>.Handle(TInput input)
    {
        return Unit.Task;
    }

    public void Handle(TInput input, Action next) { }

    Task IChainedHandler<TInput, Task>.Handle(TInput input, Func<Task> next)
    {
        return Task.CompletedTask;
    }

    Task<Unit> IChainedHandler<TInput, Task<Unit>>.Handle(TInput input, Func<Task<Unit>> next)
    {
        return Unit.Task;
    }
}
