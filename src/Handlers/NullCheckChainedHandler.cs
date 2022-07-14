namespace Kantaiko.Routing.Handlers;

public class NullCheckChainedHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly IChainedHandler<TInput, TOutput> _handler;

    public NullCheckChainedHandler(IChainedHandler<TInput, TOutput> handler)
    {
        _handler = handler;
    }

    public TOutput Handle(TInput input, Func<TInput, TOutput> next)
    {
        ArgumentNullException.ThrowIfNull(input);

        return _handler.Handle(input, next);
    }
}
