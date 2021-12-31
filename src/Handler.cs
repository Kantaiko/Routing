using System.Collections.Immutable;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing;

public static class Handler
{
    public static IHandler<TInput, TOutput> Split<TInput, TOutput>(
        SplitHandler<TInput, TOutput>.SplitPredicate splitPredicate,
        IHandler<TInput, TOutput> firstHandler,
        IHandler<TInput, TOutput> secondHandler)
    {
        ArgumentNullException.ThrowIfNull(splitPredicate);
        ArgumentNullException.ThrowIfNull(firstHandler);
        ArgumentNullException.ThrowIfNull(secondHandler);

        return new SplitHandler<TInput, TOutput>(splitPredicate, firstHandler, secondHandler);
    }

    public static IHandler<TInput, TOutput> Function<TInput, TOutput>(
        FunctionHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new FunctionHandler<TInput, TOutput>(functionDelegate);
    }

    public static IHandler<TInput, TOutput> Function<TTargetInput, TInput, TOutput>(
        FunctionHandler<TTargetInput, TOutput>.FunctionDelegate functionDelegate) where TTargetInput : TInput
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new FunctionHandler<TInput, TOutput>(input => functionDelegate((TTargetInput) input!));
    }

    public static IChainedHandler<TInput, TOutput> Function<TInput, TOutput>(
        ChainedFunctionHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new ChainedFunctionHandler<TInput, TOutput>(functionDelegate);
    }

    public static IChainedHandler<TInput, TOutput> Function<TTargetInput, TInput, TOutput>(
        ChainedFunctionHandler<TTargetInput, TOutput>.FunctionDelegate functionDelegate) where TTargetInput : TInput
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new ChainedFunctionHandler<TInput, TOutput>((input, next) =>
            functionDelegate((TTargetInput) input!, x => next(x)));
    }

    public static IHandler<TInput, TOutput> Router<TInput, TOutput>(
        IReadOnlyDictionary<Type, IHandler<TInput, TOutput>> routes)
    {
        ArgumentNullException.ThrowIfNull(routes);

        return new RouterHandler<TInput, TOutput>(routes);
    }

    public static IChainedHandler<TInput, TOutput> Chain<TInput, TOutput>(
        IEnumerable<IHandler<TInput, TOutput>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new ChainHandler<TInput, TOutput>(handlers);
    }

    public static IChainedHandler<TInput, TOutput> Transient<TInput, TOutput>(Type handlerType,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(handlerType);

        return new TransientHandler<TInput, TOutput>(handlerType, handlerFactory);
    }

    public static IReadOnlyList<IChainedHandler<TInput, TOutput>> TransientRange<TInput, TOutput>(
        IEnumerable<Type> handlerTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(handlerTypes);

        return handlerTypes.Select(x => new TransientHandler<TInput, TOutput>(x, handlerFactory)).ToImmutableArray();
    }

    public static IChainedHandler<TInput, TOutput> Transient<TInput, TOutput, THandler>(
        IHandlerFactory? handlerFactory = null)
        where THandler : IHandler<TInput, TOutput>
    {
        return new TransientHandler<TInput, TOutput>(typeof(THandler), handlerFactory);
    }

    public static IHandler<TInput, TOutput> Cast<TTargetInput, TInput, TOutput>(IHandler<TTargetInput, TOutput> handler)
        where TTargetInput : TInput
    {
        ArgumentNullException.ThrowIfNull(handler);

        return new CastHandler<TTargetInput, TInput, TOutput>(handler);
    }

    public static IHandler<TInput, TOutput> Wrap<TInput, TOutput>(this IHandler<TInput, TOutput> originalHandler,
        WrappedHandler<TInput, TOutput>.WrapperFunction wrapperFunction)
    {
        ArgumentNullException.ThrowIfNull(originalHandler);
        ArgumentNullException.ThrowIfNull(wrapperFunction);

        return new WrappedHandler<TInput, TOutput>(originalHandler, wrapperFunction);
    }

    public static IHandler<TInput, TOutput> CheckNull<TInput, TOutput>(this IHandler<TInput, TOutput> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        return new NullCheckHandler<TInput, TOutput>(handler);
    }

    public static IHandler<TInput, Unit> Parallel<TInput>(IEnumerable<IHandler<TInput, Unit>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new ParallelHandler<TInput>(handlers);
    }

    public static IHandler<TInput, Task<Unit>> ParallelAsync<TInput>(IEnumerable<IHandler<TInput, Task<Unit>>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new ParallelAsyncHandler<TInput>(handlers);
    }

    public static IHandler<TInput, Unit> Sequential<TInput>(IEnumerable<IHandler<TInput, Unit>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new SequentialHandler<TInput>(handlers);
    }

    public static IHandler<TInput, Task<Unit>> SequentialAsync<TInput>(
        IEnumerable<IHandler<TInput, Task<Unit>>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new SequentialAsyncHandler<TInput>(handlers);
    }

    public static IChainedHandler<TInput, Unit> Empty<TInput>() => EmptyHandler<TInput>.Instance;

    public static IChainedHandler<TInput, Task<Unit>> EmptyAsync<TInput>() => EmptyAsyncHandler<TInput>.Instance;
}
