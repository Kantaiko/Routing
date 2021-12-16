using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.Handlers;

public class TransientHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly Type _type;
    private readonly IHandlerFactory _handlerFactory;

    public TransientHandler(Type type, IHandlerFactory? handlerFactory = null)
    {
        _type = type;
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public TOutput Handle(TInput input, Func<TInput, TOutput> next)
    {
        var handlerInstance = _handlerFactory.CreateHandler<TInput, TOutput>(_type, input);

        if (handlerInstance is IChainedHandler<TInput, TOutput> chainedHandler)
        {
            return chainedHandler.Handle(input, next);
        }

        return handlerInstance.Handle(input);
    }

    public TOutput Handle(TInput input)
    {
        var handlerInstance = _handlerFactory.CreateHandler<TInput, TOutput>(_type, input);
        return handlerInstance.Handle(input);
    }
}
