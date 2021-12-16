namespace Kantaiko.Routing.Handlers;

public class ParallelHandler<TInput> : IHandler<TInput, Unit>
{
    private readonly IEnumerable<IHandler<TInput, Unit>> _handlers;

    public ParallelHandler(IEnumerable<IHandler<TInput, Unit>> handlers)
    {
        _handlers = handlers;
    }

    public Unit Handle(TInput input)
    {
        Parallel.ForEach(_handlers, x => x.Handle(input));

        return default;
    }
}
