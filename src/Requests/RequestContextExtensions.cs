using Kantaiko.Properties;

namespace Kantaiko.Routing.Requests;

public static class RequestContextExtensions
{
    public static IRequestContext<TRequest> WithRequest<TRequest>(this IRequestContext<TRequest> context,
        TRequest request)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(request,
            context.ServiceProvider,
            context.Properties,
            context.CancellationToken);
    }

    public static IRequestContext<TRequest> WithServiceProvider<TRequest>(this IRequestContext<TRequest> context,
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(context.Request,
            serviceProvider,
            context.Properties,
            context.CancellationToken);
    }

    public static IRequestContext<TRequest> WithCancellationToken<TRequest>(this IRequestContext<TRequest> context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(context.Request,
            context.ServiceProvider,
            context.Properties,
            cancellationToken);
    }

    public static IRequestContext<TRequest> WithProperties<TRequest>(this IRequestContext<TRequest> context,
        IReadOnlyPropertyCollection properties)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new RequestContext<TRequest>(context.Request,
            context.ServiceProvider,
            properties,
            context.CancellationToken);
    }
}
