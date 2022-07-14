using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.Handlers;

public class PolymorphicTransientHandler<TTargetInput, TInput, TOutput> : IHandler<TInput, TOutput>
    where TTargetInput : TInput
{
    private readonly Type _type;
    private readonly IHandlerFactory _handlerFactory;

    public PolymorphicTransientHandler(Type type, IHandlerFactory? handlerFactory)
    {
        _type = type;
        _handlerFactory = handlerFactory ?? DefaultHandlerFactory.Instance;
    }

    public TOutput Handle(TInput input)
    {
        var handler = _handlerFactory.CreateHandler<TTargetInput, TOutput>(_type, _handlerFactory);

        return handler.Handle((TTargetInput?) input!);
    }
}
