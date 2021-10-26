namespace Kantaiko.Routing.Handlers;

public class WrappedHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    public delegate TOutput WrapperFunction(TInput input, Func<TInput, TOutput> original);

    private readonly IHandler<TInput, TOutput> _originalHandler;
    private readonly WrapperFunction _wrapperFunction;

    public WrappedHandler(IHandler<TInput, TOutput> originalHandler, WrapperFunction wrapperFunction)
    {
        _originalHandler = originalHandler;
        _wrapperFunction = wrapperFunction;
    }

    public TOutput Handle(TInput input)
    {
        return _wrapperFunction.Invoke(input, _originalHandler.Handle);
    }
}
