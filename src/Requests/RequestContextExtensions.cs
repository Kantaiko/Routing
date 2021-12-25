namespace Kantaiko.Routing.Requests;

public static class RequestContextExtensions
{
    public static IRequestContext<TRequest> WithRequest<TRequest>(this IRequestContext<TRequest> context,
        TRequest request) where TRequest : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(request, context.ServiceProvider, context.CancellationToken);
    }

    public static IRequestContext<TRequest> WithServiceProvider<TRequest>(this IRequestContext<TRequest> context,
        IServiceProvider serviceProvider) where TRequest : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(context.Request, serviceProvider, context.CancellationToken);
    }

    public static IRequestContext<TRequest> WithCancellationToken<TRequest>(this IRequestContext<TRequest> context,
        CancellationToken cancellationToken) where TRequest : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(context.Request, context.ServiceProvider, cancellationToken);
    }
}
