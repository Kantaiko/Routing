namespace Kantaiko.Routing;

public interface IChainedHandler<TInput, TOutput> : IHandler<TInput, TOutput>
{
    TOutput Handle(TInput input, Func<TInput, TOutput> next);

    private static TOutput EmptyNext(TInput input)
    {
        throw new InvalidOperationException("No next handler was provided");
    }

    TOutput IHandler<TInput, TOutput>.Handle(TInput input) => Handle(input, EmptyNext);
}
