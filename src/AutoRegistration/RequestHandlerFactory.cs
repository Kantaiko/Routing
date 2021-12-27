using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.AutoRegistration.Exceptions;
using Kantaiko.Routing.Handlers;
using Kantaiko.Routing.Requests;

namespace Kantaiko.Routing.AutoRegistration;

public static class RequestHandlerFactory
{
    public static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateSingleRequestHandler<TRequestBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TRequestBase : class
    {
        return CreateRequestHandler<TRequestBase>((handlers, inputType) =>
        {
            if (handlers.Count == 0)
            {
                throw new MissingRequestHandlerException(inputType);
            }

            if (handlers.Count > 1)
            {
                throw new AmbiguousRequestHandlerException(inputType);
            }

            return handlers[0];
        }, types, handlerFactory);
    }

    public static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateChainedRequestHandler<TRequestBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null,
        IHandler<IRequestContext<TRequestBase>, Task<object>>? lastHandler = null)
        where TRequestBase : class
    {
        return CreateRequestHandler<TRequestBase>((handlers, _) =>
        {
            if (lastHandler is null)
            {
                return Handler.Chain(handlers);
            }

            var handler = new CastHandler<IRequestContext<TRequestBase>,
                IRequestContext<object>, Task<object>>(lastHandler);

            return Handler.Chain(handlers.Append(handler));
        }, types, handlerFactory);
    }

    private static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateRequestHandler<TRequestBase>(
        Func<
            IReadOnlyList<IHandler<IRequestContext<object>, Task<object>>>,
            Type,
            IHandler<IRequestContext<object>, Task<object>>
        > createHandler,
        IEnumerable<Type> types,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(types);

        var typeCollection = AutoRegistrationUtils.MaterializeCollection(types);

        var typeFilter = typeCollection.Where(x => x != typeof(TRequestBase));
        var inputTypes = AutoRegistrationUtils.GetInputTypes<TRequestBase>(typeFilter);

        var routes = new Dictionary<Type, IHandler<IRequestContext<TRequestBase>, Task<object>>>();

        foreach (var inputType in inputTypes)
        {
            var keyType = typeof(RequestContext<>).MakeGenericType(inputType);

            var handlers = AutoRegistrationUtils.CreateTransientHandlers<IRequestContext<object>, Task<object>>(
                inputType, typeCollection, handlerFactory);

            routes[keyType] = (IHandler<IRequestContext<TRequestBase>, Task<object>>)
                createHandler(handlers, inputType);
        }

        return Handler.Router(routes);
    }
}
