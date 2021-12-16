using System.Reflection;
using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.AutoRegistration;

public static class HandlerAutoRegistrationService
{
    public static IReadOnlyList<IHandler<TInput, TOutput>> GetTransientHandlers<TInput, TOutput>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(types);

        var handlerTypes = types.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(typeof(IAutoRegistrableHandler)) &&
            x.IsAssignableTo(typeof(IHandler<TInput, TOutput>)));

        return Handler.TransientRange<TInput, TOutput>(handlerTypes, handlerFactory);
    }

    public static IReadOnlyList<IHandler<TInput, TOutput>> GetTransientHandlers<TInput, TOutput>(
        IEnumerable<Assembly> assemblies, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        return GetTransientHandlers<TInput, TOutput>(assemblies.SelectMany(x => x.GetTypes()), handlerFactory);
    }

    public static IReadOnlyList<IHandler<TInput, TOutput>> GetTransientHandlers<TInput, TOutput>(
        Assembly assembly, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return GetTransientHandlers<TInput, TOutput>(assembly.GetTypes(), handlerFactory);
    }
}
