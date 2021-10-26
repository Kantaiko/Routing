namespace Kantaiko.Routing.Context;

public class ContextBase : IHasServiceProvider, IHasCancellationToken
{
    public ContextBase(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
    }

    public IServiceProvider ServiceProvider { get; }
    public CancellationToken CancellationToken { get; }
}
