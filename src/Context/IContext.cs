using Kantaiko.Properties;
using Kantaiko.Properties.Immutable;

namespace Kantaiko.Routing.Context;

public interface IContext : IHasServiceProvider, IReadOnlyPropertyContainer, IHasCancellationToken
{
    new IImmutablePropertyCollection Properties { get; }

    IReadOnlyPropertyCollection IReadOnlyPropertyContainer.Properties => Properties;
}
