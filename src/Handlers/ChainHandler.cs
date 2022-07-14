namespace Kantaiko.Routing.Handlers;

public class ChainHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly IEnumerable<IChainedHandler<TInput, TOutput>> _chainedHandlers;

    public ChainHandler(IEnumerable<IChainedHandler<TInput, TOutput>> chainedHandlers)
    {
        _chainedHandlers = chainedHandlers;
    }

    public TOutput Handle(TInput input, Func<TInput, TOutput> next)
    {
        var handlerEnumerator = _chainedHandlers.GetEnumerator();

        TOutput MoveNext(TInput mContext)
        {
            if (!handlerEnumerator.MoveNext())
            {
                handlerEnumerator.Dispose();
                return next(mContext);
            }

            var result = handlerEnumerator.Current.Handle(mContext, MoveNext);

            if (result is Task task)
            {
                task.ContinueWith(_ => handlerEnumerator.Dispose());
            }
            else
            {
                handlerEnumerator.Dispose();
            }

            return result;
        }

        return MoveNext(input);
    }
}
