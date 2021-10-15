namespace Kantaiko.Routing.Handlers;

public class ChainedFunctionHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    public delegate TOutput FunctionDelegate(TInput input, Func<TInput, TOutput> next);

    private readonly FunctionDelegate _functionDelegate;

    public ChainedFunctionHandler(FunctionDelegate functionDelegate)
    {
        _functionDelegate = functionDelegate;
    }

    public TOutput Handle(TInput input, Func<TInput, TOutput> next) => _functionDelegate(input, next);
}