namespace Kantaiko.Routing.Context;

public class AsyncContextBase : ContextBase, IAsyncContext
{
    public AsyncContextBase(
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default
    ) : base(serviceProvider)
    {
        CancellationToken = cancellationToken;
    }

    public CancellationToken CancellationToken { get; }
}
