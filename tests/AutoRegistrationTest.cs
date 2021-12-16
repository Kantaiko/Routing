using System.Reflection;
using Kantaiko.Routing.AutoRegistration;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class AutoRegistrationTest
{
    private abstract class TestChainHandler : IChainedHandler<int, int>, IAutoRegistrableHandler
    {
        public abstract int Handle(int input, Func<int, int> next);
    }

    private class AddOneHandler : TestChainHandler
    {
        public override int Handle(int input, Func<int, int> next) => next(input + 1);
    }

    private class AddTwoHandler : TestChainHandler
    {
        public override int Handle(int input, Func<int, int> next) => next(input + 2);
    }

    [Fact]
    public void ShouldGetAssemblyTypes()
    {
        var handlers = HandlerAutoRegistrationService.GetTransientHandlers<int, int>(Assembly.GetExecutingAssembly());

        var lastHandler = Handler.Function<int, int>(input => input);
        var handler = Handler.Chain(handlers.Append(lastHandler));

        Assert.Equal(3, handler.Handle(0));
    }
}
