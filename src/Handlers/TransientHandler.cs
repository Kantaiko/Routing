using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.Handlers;

public class TransientHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    private readonly Type _type;
    private readonly IHandlerFactory _handlerFactory;

    public TransientHandler(Type type, IHandlerFactory? handlerFactory = null)
    {
        _type = type;
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public TOutput Handle(TInput input)
    {
        var handlerInstance = _handlerFactory.CreateHandler<TInput, TOutput>(_type, input);
        return handlerInstance.Handle(input);
    }
}
