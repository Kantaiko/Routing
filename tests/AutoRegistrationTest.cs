using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class AutoRegistrationTest
{
    public abstract class TestChainHandler : IChainedHandler<int, int>, IAutoRegistrableHandler
    {
        public abstract int Handle(int input, Func<int, int> next);
    }

    public class AddOneHandler : TestChainHandler
    {
        public override int Handle(int input, Func<int, int> next) => next(input + 1);
    }

    public class AddTwoHandler : TestChainHandler
    {
        public override int Handle(int input, Func<int, int> next) => next(input + 2);
    }

    [Fact]
    public void ShouldGetAssemblyTypes()
    {
        var types = typeof(AutoRegistrationTest).GetNestedTypes();
        var handlers = HandlerFactory.CreateTransientChainedHandlers<int, int>(types);

        var chainHandler = new ChainHandler<int, int>(handlers);

        Assert.Equal(3, chainHandler.Handle(0, x => x));
    }
}
