using Kantaiko.Routing.Exceptions;

namespace Kantaiko.Routing.Handlers;

public class ChainHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    private readonly IEnumerable<IHandler<TInput, TOutput>> _chainedHandlers;

    public ChainHandler(IEnumerable<IHandler<TInput, TOutput>> chainedHandlers)
    {
        _chainedHandlers = chainedHandlers;
    }

    public TOutput Handle(TInput input)
    {
        var handlerEnumerator = _chainedHandlers.GetEnumerator();

        TOutput MoveNext(TInput mContext)
        {
            if (!handlerEnumerator.MoveNext())
            {
                handlerEnumerator.Dispose();
                throw new ChainEndedException();
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
}
