namespace Kantaiko.Routing.Handlers;

public class NullCheckHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    private readonly IHandler<TInput, TOutput> _handler;

    public NullCheckHandler(IHandler<TInput, TOutput> handler)
    {
        _handler = handler;
    }

    public TOutput Handle(TInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return _handler.Handle(input);
    }
}
