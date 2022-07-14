namespace Kantaiko.Routing.Context;

public class ContextBase : IContext
{
    public ContextBase(IServiceProvider? serviceProvider = null)
    {
        if (serviceProvider?.GetService(typeof(IContextAcceptor)) is IContextAcceptor contextAcceptor)
        {
            contextAcceptor.SetContext(this);
        }

        ServiceProvider = serviceProvider ?? DefaultServiceProvider.Instance;
    }

    public IServiceProvider ServiceProvider { get; }
}
