using Kantaiko.Routing.Exceptions;

namespace Kantaiko.Routing.Handlers;

public class ChainHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly IEnumerable<IHandler<TInput, TOutput>> _chainedHandlers;

    public ChainHandler(IEnumerable<IHandler<TInput, TOutput>> chainedHandlers)
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

            var result = handlerEnumerator.Current is IChainedHandler<TInput, TOutput> chainedHandler
                ? chainedHandler.Handle(mContext, MoveNext)
                : handlerEnumerator.Current.Handle(mContext);

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

    private static TOutput EndNext(TInput input)
    {
        throw new ChainEndedException();
    }

    public TOutput Handle(TInput input)
    {
        return Handle(input, EndNext);
    }
}
