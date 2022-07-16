using System.Runtime.ExceptionServices;

namespace Kantaiko.Routing.Handlers;

public abstract class ChainedContextHandlerBase<TContext> : IChainedHandler<TContext, Unit>
{
    protected TContext Context { get; private set; } = default!;

    Unit IChainedHandler<TContext, Unit>.Handle(TContext input, Func<Unit> next)
    {
        Context = input;

        BeforeHandle(input);

        try
        {
            Handle(input, next);
        }
        catch (Exception exception)
        {
            AfterHandle(input, exception);
            return default;
        }

        AfterHandle(input, null);
        return default;
    }

    protected abstract Unit Handle(TContext context, Func<Unit> next);

    /// <summary>
    /// Invokes before HandleAsync in order to perform pre-processing operations.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual void BeforeHandle(TContext context) { }

    /// <summary>
    /// Invokes after HandleAsync in order to perform post-processing operations.
    /// This method also handles the exception if any.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual void AfterHandle(TContext context, Exception? exception)
    {
        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
