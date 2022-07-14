namespace Kantaiko.Routing.Handlers;

public class ParallelAsyncHandler<TInput> : IHandler<TInput, Task>
{
    private readonly IEnumerable<IHandler<TInput, Task>> _handlers;

    public ParallelAsyncHandler(IEnumerable<IHandler<TInput, Task>> handlers)
    {
        _handlers = handlers;
    }

    public Task Handle(TInput input)
    {
        return Task.WhenAll(_handlers.Select(x => x.Handle(input)));
    }
}
