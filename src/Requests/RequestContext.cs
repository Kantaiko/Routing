using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Requests;

public class RequestContext<TRequest> : ContextBase, IRequestContext<TRequest>
{
    public RequestContext(TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken) :
        base(serviceProvider, cancellationToken)
    {
        Request = request;
    }

    public TRequest Request { get; }
}
