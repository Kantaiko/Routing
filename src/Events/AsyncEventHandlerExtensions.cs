namespace Kantaiko.Routing.Events;

public static class AsyncEventHandlerExtensions
{
    /// <summary>
    /// Invokes registered event handlers sequentially.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <param name="context"></param>
    /// <typeparam name="TContext"></typeparam>
    public static async Task InvokeAsync<TContext>(this AsyncEventHandler<TContext>? eventHandler, TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (eventHandler is null)
        {
            return;
        }

        var handlers = eventHandler.GetInvocationList()
            .OfType<AsyncEventHandler<TContext>>();

        foreach (var handler in handlers)
        {
            await handler.Invoke(context);
        }
    }

    /// <summary>
    /// Invokes registered event handlers parallel.
    /// </summary>
    /// <param name="eventHandler"></param>
    /// <param name="context"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static Task InvokeParallelAsync<TContext>(this AsyncEventHandler<TContext>? eventHandler,
        TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (eventHandler is null)
        {
            return Task.CompletedTask;
        }

        var handlerTasks = eventHandler.GetInvocationList()
            .OfType<AsyncEventHandler<TContext>>()
            .Select(x => x.Invoke(context));

        return Task.WhenAll(handlerTasks);
    }
}
