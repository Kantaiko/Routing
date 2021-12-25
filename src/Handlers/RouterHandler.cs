namespace Kantaiko.Routing.Handlers;

public class RouterHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    private readonly IReadOnlyDictionary<Type, IHandler<TInput, TOutput>> _routes;

    public RouterHandler(IReadOnlyDictionary<Type, IHandler<TInput, TOutput>> routes)
    {
        _routes = routes;
    }

    public TOutput Handle(TInput input)
    {
        return _routes[input!.GetType()].Handle(input);
    }
}
