using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Exceptions;

namespace Kantaiko.Routing.Handlers;

public class TransientChainHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    private readonly IEnumerable<Type> _chainedHandlerTypes;
    private readonly IHandlerFactory _handlerFactory;

    public TransientChainHandler(IEnumerable<Type> chainedHandlerTypes, IHandlerFactory? handlerFactory = null)
    {
        _chainedHandlerTypes = chainedHandlerTypes;
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public TOutput Handle(TInput input)
    {
        var handlerTypeEnumerator = _chainedHandlerTypes.GetEnumerator();

        TOutput MoveNext(TInput mContext)
        {
            if (!handlerTypeEnumerator.MoveNext())
            {
                handlerTypeEnumerator.Dispose();
                throw new ChainEndedException();
            }

            var handler = _handlerFactory.CreateHandler<TInput, TOutput>(handlerTypeEnumerator.Current, input);

            var result = handler is IChainedHandler<TInput, TOutput> chainedHandler
                ? chainedHandler.Handle(mContext, MoveNext)
                : handler.Handle(mContext);

            if (result is Task task)
            {
                task.ContinueWith(_ => handlerTypeEnumerator.Dispose());
            }
            else
            {
                handlerTypeEnumerator.Dispose();
            }

            return result;
        }

        return MoveNext(input);
    }
}
