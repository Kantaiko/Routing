using Kantaiko.Properties;
using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Requests;

public class RequestContext<TRequest> : ContextBase, IRequestContext<TRequest>
{
    public RequestContext(TRequest request,
        IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null,
        CancellationToken cancellationToken = default) :
        base(serviceProvider, properties, cancellationToken)
    {
        Request = request;
    }

    public RequestContext(TRequest request)
    {
        Request = request;
    }

    public TRequest Request { get; }
}
