namespace Kantaiko.Routing.Context;

public class ContextAccessor<TContext> : IContextAccessor<TContext> where TContext : IContext
{
    private readonly ContextAccessor _contextAccessor;

    public ContextAccessor(ContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public TContext? Context => _contextAccessor.Context switch
    {
        null => default,
        TContext context => context,
        _ => throw new InvalidOperationException(
            $"Unable to access context of type \"{typeof(TContext).Name}\"." +
            $"The current context type is \"{_contextAccessor.Context.GetType().Name}\"")
    };
}

public class ContextAccessor : IContextAccessor, IContextAcceptor
{
    public IContext? Context { get; private set; }

    public void SetContext(IContext context)
    {
        Context = context;
    }
}
