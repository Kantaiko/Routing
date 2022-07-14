namespace Kantaiko.Routing.Context;

public class ContextAccessor : IContextAccessor, IContextAcceptor
{
    public object? Context { get; private set; }

    public void SetContext(object context)
    {
        if (Context is not null)
        {
            throw new InvalidOperationException(
                "The current scope already has context. " +
                "Do not use multiple contexts in the same scope."
            );
        }

        Context = context;
    }
}

public readonly struct ContextAccessor<TContext>
{
    private readonly ContextAccessor _contextAccessor;

    public ContextAccessor(ContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public TContext Context => _contextAccessor.Context switch
    {
        TContext context => context,
        null => throw new InvalidOperationException(
            $"Unable to access context of type \"{typeof(TContext).Name}\"." +
            "There is no any context in the current scope"
        ),
        _ => throw new InvalidOperationException(
            $"Unable to access context of type \"{typeof(TContext).Name}\"." +
            $"The current context type is \"{_contextAccessor.Context.GetType().Name}\""
        )
    };
}
