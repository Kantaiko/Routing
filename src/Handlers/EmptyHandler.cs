namespace Kantaiko.Routing.Handlers;

public class EmptyHandler<TInput> : IHandler<TInput, Unit>
{
    public Unit Handle(TInput input) => default;

    public static EmptyHandler<TInput> Instance { get; } = new();
}
