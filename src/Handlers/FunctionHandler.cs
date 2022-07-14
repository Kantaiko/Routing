namespace Kantaiko.Routing.Handlers;

public class FunctionHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    public delegate TOutput FunctionDelegate(TInput input);

    private readonly FunctionDelegate _functionDelegate;

    public FunctionHandler(FunctionDelegate functionDelegate)
    {
        _functionDelegate = functionDelegate;
    }

    public TOutput Handle(TInput input)
    {
        return _functionDelegate(input);
    }
}
