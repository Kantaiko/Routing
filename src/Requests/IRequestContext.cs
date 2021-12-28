using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Requests;

public interface IRequestContext<out TRequest> : IContext
{
    TRequest Request { get; }
}
