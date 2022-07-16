using System.Runtime.ExceptionServices;

namespace Kantaiko.Routing.Handlers;

public abstract class ContextHandlerBase<TContext> : IHandler<TContext>
{
    protected TContext Context { get; private set; } = default!;

    void IHandler<TContext>.Handle(TContext input)
    {
        Context = input;

        BeforeHandle(input);

        try
        {
            Handle(input);
        }
        catch (Exception exception)
        {
            AfterHandle(input, exception);
        }

        AfterHandle(input, null);
    }

    protected abstract void Handle(TContext context);

    /// <summary>
    /// Invokes before Handle in order to perform pre-processing operations.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual void BeforeHandle(TContext context) { }

    /// <summary>
    /// Invokes after Handle in order to perform post-processing operations.
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
