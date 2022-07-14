using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing;

public static class Handler
{
    public static IHandler<TInput, TOutput> Router<TInput, TOutput>(
        IReadOnlyDictionary<Type, IHandler<TInput, TOutput>> routes)
    {
        ArgumentNullException.ThrowIfNull(routes);

        return new RouterHandler<TInput, TOutput>(routes);
    }

    public static IHandler<TInput, TOutput> Function<TInput, TOutput>(
        FunctionHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new FunctionHandler<TInput, TOutput>(functionDelegate);
    }

    public static IChainedHandler<TInput, TOutput> Function<TInput, TOutput>(
        FunctionChainedHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
    {
        ArgumentNullException.ThrowIfNull(functionDelegate);

        return new FunctionChainedHandler<TInput, TOutput>(functionDelegate);
    }

    public static IChainedHandler<TInput, TOutput> Chain<TInput, TOutput>(
        IEnumerable<IChainedHandler<TInput, TOutput>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new ChainHandler<TInput, TOutput>(handlers);
    }

    public static IChainedHandler<TInput, TOutput> WithNullCheck<TInput, TOutput>(
        this IChainedHandler<TInput, TOutput> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        return new NullCheckChainedHandler<TInput, TOutput>(handler);
    }

    public static IHandler<TInput, TOutput> WithNullCheck<TInput, TOutput>(this IHandler<TInput, TOutput> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        return new NullCheckHandler<TInput, TOutput>(handler);
    }

    public static IHandler<TInput, Task> ParallelAsync<TInput>(IEnumerable<IHandler<TInput, Task>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new ParallelAsyncHandler<TInput>(handlers);
    }

    public static IHandler<TInput, Task> SequentialAsync<TInput>(
        IEnumerable<IHandler<TInput, Task>> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        return new SequentialAsyncHandler<TInput>(handlers);
    }

    public static IHandler<TInput, Task> EmptyAsync<TInput>() => EmptyAsyncHandler<TInput>.Instance;

    public static IChainedHandler<TInput, Task> EmptyChainedAsync<TInput>() => EmptyAsyncHandler<TInput>.Instance;
}
