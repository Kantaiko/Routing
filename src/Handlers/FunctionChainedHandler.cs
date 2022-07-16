namespace Kantaiko.Routing.Handlers;

public class FunctionChainedHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    public delegate TOutput FunctionDelegate(TInput input, Func<TOutput> next);

    private readonly FunctionDelegate _functionDelegate;

    public FunctionChainedHandler(FunctionDelegate functionDelegate)
    {
        _functionDelegate = functionDelegate;
    }

    public TOutput Handle(TInput input, Func<TOutput> next)
    {
        return _functionDelegate(input, next);
    }
}
