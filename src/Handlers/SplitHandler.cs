namespace Kantaiko.Routing.Handlers;

public class SplitHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    public delegate bool SplitPredicate(TInput input);

    private readonly SplitPredicate _splitPredicate;
    private readonly IHandler<TInput, TOutput> _firstHandler;
    private readonly IHandler<TInput, TOutput> _secondHandler;

    public SplitHandler(SplitPredicate splitPredicate,
        IHandler<TInput, TOutput> firstHandler,
        IHandler<TInput, TOutput> secondHandler)
    {
        _splitPredicate = splitPredicate;
        _firstHandler = firstHandler;
        _secondHandler = secondHandler;
    }

    public TOutput Handle(TInput input)
    {
        return _splitPredicate(input)
            ? _firstHandler.Handle(input)
            : _secondHandler.Handle(input);
    }
}