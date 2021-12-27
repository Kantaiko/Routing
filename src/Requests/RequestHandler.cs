using Kantaiko.Routing.AutoRegistration;

namespace Kantaiko.Routing.Requests;

public abstract class RequestHandler<TRequest, TResponse> :
    IHandler<IRequestContext<object>, Task<object?>>,
    IAutoRegistrableHandler<TRequest>
{
    protected IRequestContext<TRequest> Context { get; private set; } = null!;

    protected TRequest Request => Context.Request;
    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    protected abstract Task<TResponse> HandleAsync(IRequestContext<TRequest> context);

    async Task<object?> IHandler<IRequestContext<object>, Task<object?>>.Handle(
        IRequestContext<object> input)
    {
        Context = (IRequestContext<TRequest>) input;

        await BeforeHandleAsync(Context);

        var response = await HandleAsync(Context);

        return await AfterHandleAsync(Context, response);
    }

    protected virtual Task BeforeHandleAsync(IRequestContext<TRequest> context) => Task.CompletedTask;

    protected virtual Task<TResponse> AfterHandleAsync(IRequestContext<TRequest> context, TResponse response) =>
        Task.FromResult(response);
}
