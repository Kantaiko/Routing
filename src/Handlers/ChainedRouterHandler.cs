namespace Kantaiko.Routing.Handlers;

public class ChainedRouterHandler<TInput, TOutput> : IChainedHandler<TInput, TOutput>
{
    private readonly IReadOnlyDictionary<Type, IChainedHandler<TInput, TOutput>> _routes;

    public ChainedRouterHandler(IReadOnlyDictionary<Type, IChainedHandler<TInput, TOutput>> routes)
    {
        _routes = routes;
    }

    public TOutput Handle(TInput input, Func<TOutput> next)
    {
        return _routes[input!.GetType()].Handle(input, next);
    }
}
