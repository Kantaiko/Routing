using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.Handlers;

public class TransientHandler<TInput, TOutput> : IHandler<TInput, TOutput>, IChainedHandler<TInput, TOutput>
{
    private readonly Type _type;
    private readonly IHandlerFactory _handlerFactory;

    public TransientHandler(Type type, IHandlerFactory? handlerFactory = null)
    {
        _type = type;
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public TOutput Handle(TInput input, Func<TOutput> next)
    {
        var handlerInstance = _handlerFactory.CreateChainedHandler<TInput, TOutput>(_type, input);

        return handlerInstance.Handle(input, next);
    }

    public TOutput Handle(TInput input)
    {
        var handlerInstance = _handlerFactory.CreateHandler<TInput, TOutput>(_type, input);

        return handlerInstance.Handle(input);
    }
}
