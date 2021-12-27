using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.AutoRegistration.Exceptions;
using Kantaiko.Routing.Requests;

namespace Kantaiko.Routing.AutoRegistration;

public static class RequestHandlerFactory
{
    public static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateSingleRequestHandler<TRequestBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TRequestBase : IRequestBase
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
        IHandler<IRequestContext<IRequestBase>, Task<object>>? lastHandler = null) where TRequestBase : IRequestBase
    {
        return CreateRequestHandler<TRequestBase>(
            (handlers, _) => Handler.Chain(lastHandler is not null ? handlers.Append(lastHandler) : handlers),
            types, handlerFactory);
    }

    private static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateRequestHandler<TRequestBase>(
        Func<
            IReadOnlyList<IHandler<IRequestContext<IRequestBase>, Task<object>>>,
            Type,
            IHandler<IRequestContext<IRequestBase>, Task<object>>
        > createHandler,
        IEnumerable<Type> types,
        IHandlerFactory? handlerFactory = null) where TRequestBase : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(types);

        var typeCollection = AutoRegistrationUtils.MaterializeCollection(types);

        var typeFilter = typeCollection.Where(x => x != typeof(TRequestBase));
        var inputTypes = AutoRegistrationUtils.GetInputTypes<TRequestBase>(typeFilter);

        var routes = new Dictionary<Type, IHandler<IRequestContext<TRequestBase>, Task<object>>>();

        foreach (var inputType in inputTypes)
        {
            var keyType = typeof(RequestContext<>).MakeGenericType(inputType);

            var handlers = AutoRegistrationUtils.CreateTransientHandlers<IRequestContext<IRequestBase>, Task<object>>(
                inputType, typeCollection, handlerFactory);

            routes[keyType] = (IHandler<IRequestContext<TRequestBase>, Task<object>>)
                createHandler(handlers, inputType);
        }

        return Handler.Router(routes);
    }
}
