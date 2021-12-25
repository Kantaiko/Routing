namespace Kantaiko.Routing.Handlers;

public class EmptyHandler<TInput> : IChainedHandler<TInput, Unit>
{
    public Unit Handle(TInput input, Func<TInput, Unit> next) => default;

    public Unit Handle(TInput input) => default;

    public static EmptyHandler<TInput> Instance { get; } = new();
}
