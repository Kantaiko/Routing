using System.Reflection;
using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.AutoRegistration.Exceptions;
using Kantaiko.Routing.Requests;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class RequestHandlerTest
{
    [Fact]
    public async Task ShouldHandleRequestUsingRequestHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var handler = RequestHandlerFactory.CreateSingleRequestHandler<ITestRequestBaseA>(types);

        var requestA = new TestRequestA();

        var result = await handler.HandleAsync<TestRequestA, int>(new RequestContext<TestRequestA>(requestA,
            DefaultServiceProvider.Instance, CancellationToken.None));

        Assert.Equal(42, result);
    }

    [Fact]
    public void ShouldReportMissingRequestHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        void Action()
        {
            RequestHandlerFactory.CreateSingleRequestHandler<ITestRequestBaseB>(types);
        }

        Assert.Throws<MissingRequestHandlerException>(Action);
    }

    [Fact]
    public void ShouldReportMultipleRequestHandlers()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        void Action()
        {
            RequestHandlerFactory.CreateSingleRequestHandler<ITestRequestBaseC>(types);
        }

        Assert.Throws<AmbiguousRequestHandlerException>(Action);
    }

    private interface ITestRequestBaseA : IRequestBase { }

    private interface ITestRequestBaseB : IRequestBase { }

    private interface ITestRequestBaseC : IRequestBase { }

    private class TestRequestA : IRequest<int>, ITestRequestBaseA { }

    private class TestRequestB : IRequest<int>, ITestRequestBaseB { }

    private class TestRequestC : IRequest<int>, ITestRequestBaseC { }

    private class TestRequestAHandler : RequestHandler<TestRequestA, int>
    {
        protected override Task<int> HandleAsync(IRequestContext<TestRequestA> context)
        {
            return Task.FromResult(42);
        }
    }

    private class TestRequestCHandler1 : RequestHandler<TestRequestC, int>
    {
        protected override Task<int> HandleAsync(IRequestContext<TestRequestC> context)
        {
            throw new InvalidOperationException();
        }
    }

    private class TestRequestCHandler2 : RequestHandler<TestRequestC, int>
    {
        protected override Task<int> HandleAsync(IRequestContext<TestRequestC> context)
        {
            throw new InvalidOperationException();
        }
    }
}
