namespace Kantaiko.Routing.Handlers;

public class SequentialAsyncHandler<TInput> : IHandler<TInput, Task<Unit>>
{
    private readonly IEnumerable<IHandler<TInput, Task<Unit>>> _handlers;

    public SequentialAsyncHandler(IEnumerable<IHandler<TInput, Task<Unit>>> handlers)
    {
        _handlers = handlers;
    }

    public async Task<Unit> Handle(TInput input)
    {
        foreach (var handler in _handlers)
        {
            await handler.Handle(input);
        }

        return default;
    }
}
