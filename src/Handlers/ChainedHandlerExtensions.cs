using Kantaiko.Routing.Exceptions;

namespace Kantaiko.Routing.Handlers;

public static class ChainedHandlerExtensions
{
    public static TOutput Handle<TOutput, TInput>(this IChainedHandler<TInput, TOutput> chainedHandler, TInput input)
    {
        ArgumentNullException.ThrowIfNull(chainedHandler);

        return chainedHandler.Handle(input, () => throw new NoHandlersLeftException());
    }
}
