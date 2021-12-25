namespace Kantaiko.Routing.Requests;

public static class ObjectHandlerExtensions
{
    public static async Task<TResponse> HandleAsync<TRequest, TResponse>(
        this IHandler<IRequestContext<TRequest>, Task<object>> handler, IRequestContext<TRequest> context)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(handler);

        return (TResponse) await handler.Handle(context);
    }
}
