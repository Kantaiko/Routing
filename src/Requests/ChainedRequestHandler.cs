using Kantaiko.Routing.AutoRegistration;

namespace Kantaiko.Routing.Requests;

public abstract class ChainedRequestHandler<TRequest, TResponse> :
    IChainedHandler<IRequestContext<object>, Task<object?>>,
    IAutoRegistrableHandler<TRequest>
    where TRequest : IRequest<TResponse>
{
    protected IRequestContext<TRequest> Context { get; private set; } = null!;

    protected TRequest Request => Context.Request;
    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    protected delegate Task<TResponse> NextAction(IRequestContext<TRequest>? context = default);

    protected abstract Task<TResponse> HandleAsync(IRequestContext<TRequest> context, NextAction next);

    async Task<object?> IChainedHandler<IRequestContext<object>, Task<object?>>.Handle(
        IRequestContext<object> input,
        Func<IRequestContext<object>, Task<object?>> next)
    {
        Context = (IRequestContext<TRequest>) input;

        await BeforeHandleAsync(Context);

        var response = await HandleAsync(Context,
            async x => (TResponse) (await next((IRequestContext<object>) (x ?? Context)))!);

        return await AfterHandleAsync(Context, response);
    }

    protected virtual Task BeforeHandleAsync(IRequestContext<TRequest> context) => Task.CompletedTask;

    protected virtual Task<TResponse> AfterHandleAsync(IRequestContext<TRequest> context, TResponse response) =>
        Task.FromResult(response);
}
