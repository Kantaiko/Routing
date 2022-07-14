namespace Kantaiko.Routing.Handlers;

public interface IChainedHandler<TInput, TOutput>
{
    TOutput Handle(TInput input, Func<TInput, TOutput> next);
}
