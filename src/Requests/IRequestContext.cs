using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Requests;

public interface IRequestContext<out TRequest> : IHasServiceProvider, IHasCancellationToken
{
    TRequest Request { get; }
}
