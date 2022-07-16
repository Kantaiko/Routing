namespace Kantaiko.Routing.Handlers;

public interface IChainedHandler<in TInput, TOutput>
{
    TOutput Handle(TInput input, Func<TOutput> next);
}

public interface IChainedHandler<in TInput> : IChainedHandler<TInput, Unit>
{
    void Handle(TInput input, Action next);

    Unit IChainedHandler<TInput, Unit>.Handle(TInput input, Func<Unit> next)
    {
        Handle(input, next);

        return default;
    }
}
