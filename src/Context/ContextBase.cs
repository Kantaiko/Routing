using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Routing.Context;

public class ContextBase : IContext
{
    public ContextBase(IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null,
        CancellationToken cancellationToken = default)
    {
        serviceProvider ??= DefaultServiceProvider.Instance;
        properties ??= ImmutablePropertyCollection.Empty;

        ServiceProvider = new ContextServiceProvider(this, serviceProvider);
        Properties = properties.ToImmutable();
        CancellationToken = cancellationToken;
    }

    public IServiceProvider ServiceProvider { get; }
    public IImmutablePropertyCollection Properties { get; }

    public CancellationToken CancellationToken { get; }
}
