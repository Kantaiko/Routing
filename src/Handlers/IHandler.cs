namespace Kantaiko.Routing.Handlers;

public interface IHandler<in TInput, out TOutput>
{
    TOutput Handle(TInput input);
}

public interface IHandler<in TInput> : IHandler<TInput, Unit>
{
    new void Handle(TInput input);

    Unit IHandler<TInput, Unit>.Handle(TInput input)
    {
        Handle(input);

        return default;
    }
}
