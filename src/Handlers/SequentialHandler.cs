namespace Kantaiko.Routing.Handlers;

public class SequentialHandler<TInput> : IHandler<TInput>
{
    private readonly IEnumerable<IHandler<TInput, Unit>> _handlers;

    public SequentialHandler(IEnumerable<IHandler<TInput, Unit>> handlers)
    {
        _handlers = handlers;
    }

    public void Handle(TInput input)
    {
        foreach (var handler in _handlers)
        {
            handler.Handle(input);
        }
    }
}
