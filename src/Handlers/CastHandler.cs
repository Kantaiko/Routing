namespace Kantaiko.Routing.Handlers;

public class CastHandler<TTargetInput, TInput, TOutput> : IHandler<TInput, TOutput> where TTargetInput : TInput
{
    private readonly IHandler<TTargetInput, TOutput> _handler;

    public CastHandler(IHandler<TTargetInput, TOutput> handler)
    {
        _handler = handler;
    }

    public TOutput Handle(TInput input)
    {
        return _handler.Handle((TTargetInput) input!);
    }
}
