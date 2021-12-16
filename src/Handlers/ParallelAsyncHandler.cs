namespace Kantaiko.Routing.Handlers;

public class ParallelAsyncHandler<TInput> : IHandler<TInput, Task<Unit>>
{
    private readonly IEnumerable<IHandler<TInput, Task<Unit>>> _handlers;

    public ParallelAsyncHandler(IEnumerable<IHandler<TInput, Task<Unit>>> handlers)
    {
        _handlers = handlers;
    }

    public async Task<Unit> Handle(TInput input)
    {
        await Task.WhenAll(_handlers.Select(x => x.Handle(input)));

        return default;
    }
}
