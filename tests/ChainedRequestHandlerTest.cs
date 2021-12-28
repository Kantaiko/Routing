using System.Reflection;
using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Requests;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class ChainedRequestHandlerTest
{
    [Fact]
    public async Task ShouldHandleRequestUsingChainedRequestHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        var lastHandler = Handler.Function<IRequestContext<ITestRequestBase>, Task<object>>((context, _) =>
            Task.FromResult<object>(context.Request.A));

        var handler = RequestHandlerFactory.CreateChainedRequestHandler(types, lastHandler: lastHandler);

        var request = new TestRequestA(20);
        var response = await handler.HandleAsync<TestRequestA, int>(new RequestContext<TestRequestA>(request));

        Assert.Equal(42, response);
    }

    private interface ITestRequestBase
    {
        public int A { get; }
    }

    private interface ITestRequest<TResponse> : IRequest<TResponse>, ITestRequestBase { }

    private record TestRequestA(int A) : ITestRequest<int>;

    private class TestRequestHandler1 : ChainedRequestHandler<TestRequestA, int>
    {
        protected override Task<int> HandleAsync(IRequestContext<TestRequestA> context, NextHandler next)
        {
            return next(context.WithRequest(context.Request with { A = context.Request.A + 20 }));
        }
    }

    private class TestRequestHandler2 : ChainedRequestHandler<TestRequestA, int>
    {
        protected override Task<int> HandleAsync(IRequestContext<TestRequestA> context, NextHandler next)
        {
            return next(context.WithRequest(context.Request with { A = context.Request.A + 2 }));
        }
    }
}
