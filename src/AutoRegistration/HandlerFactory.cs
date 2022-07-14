using System.Collections.Immutable;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration;

public static class HandlerFactory
{
    /// <summary>
    /// <para>
    /// Finds all appropriate handlers in the given type enumerable
    /// and creates a list of corresponding transient handlers.
    /// </para>
    /// Appropriate handlers must:
    /// <list type="bullet">
    /// <item>be non-abstract classes;</item>
    /// <item>implement <see cref="IHandler{TInput,TOutput}"/> interface with specified type parameters;</item>
    /// <item>implement <see cref="IAutoRegistrableHandler"/> interface.</item>
    /// </list>
    /// </summary>
    /// <param name="lookupTypes"></param>
    /// <param name="handlerFactory"></param>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static IReadOnlyList<IHandler<TInput, TOutput>> CreateTransientHandlers<TInput, TOutput>(
        IEnumerable<Type> lookupTypes, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        var handlerTypes = lookupTypes.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(typeof(IAutoRegistrableHandler)) &&
            x.IsAssignableTo(typeof(IHandler<TInput, TOutput>)));

        return handlerTypes
            .Select(x => new TransientHandler<TInput, TOutput>(x, handlerFactory))
            .ToImmutableArray();
    }

    /// <summary>
    /// <para>
    /// Finds all appropriate chained handlers in the given type enumerable
    /// and creates a list of corresponding transient handlers.
    /// </para>
    /// Appropriate chained handlers must:
    /// <list type="bullet">
    /// <item>be non-abstract classes;</item>
    /// <item>implement <see cref="IChainedHandler{TInput,TOutput}"/> interface with specified type parameters;</item>
    /// <item>implement <see cref="IAutoRegistrableHandler"/> interface.</item>
    /// </list>
    /// </summary>
    /// <param name="lookupTypes"></param>
    /// <param name="handlerFactory"></param>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static IReadOnlyList<IChainedHandler<TInput, TOutput>> CreateTransientChainedHandlers<TInput, TOutput>(
        IEnumerable<Type> lookupTypes, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        var handlerTypes = lookupTypes.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(typeof(IAutoRegistrableHandler)) &&
            x.IsAssignableTo(typeof(IChainedHandler<TInput, TOutput>)));

        return handlerTypes
            .Select(x => new TransientHandler<TInput, TOutput>(x, handlerFactory))
            .ToImmutableArray();
    }
}
