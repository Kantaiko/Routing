namespace Kantaiko.Routing.Handlers;

public class NullCheckHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly IHandler<TInput, TOutput> _handler;

    public NullCheckHandler(IHandler<TInput, TOutput> handler)
    {
        _handler = handler;
    }

    public TOutput Handle(TInput input, Func<TInput, TOutput> next)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(next);

        return _handler is IChainedHandler<TInput, TOutput> chainedHandler
            ? chainedHandler.Handle(input, next)
            : _handler.Handle(input);
    }

    public TOutput Handle(TInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return _handler.Handle(input);
    }
}
