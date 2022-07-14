namespace Kantaiko.Routing.Handlers;

public class SequentialAsyncHandler<TInput> : IHandler<TInput, Task>
{
    private readonly IEnumerable<IHandler<TInput, Task>> _handlers;

    public SequentialAsyncHandler(IEnumerable<IHandler<TInput, Task>> handlers)
    {
        _handlers = handlers;
    }

    public async Task Handle(TInput input)
    {
        foreach (var handler in _handlers)
        {
            await handler.Handle(input);
        }
    }
}
