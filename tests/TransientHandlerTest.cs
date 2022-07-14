using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class TransientHandlerTest
{
    private class TestHandlerFactory : IHandlerFactory
    {
        public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
        {
            if (handlerType == typeof(TestHandler))
            {
                return new TestHandler(42);
            }

            throw new InvalidOperationException("Unexpected service type");
        }
    }

    private class TestHandler : IHandler<Unit, int>
    {
        private readonly int _magicNumber;

        public TestHandler(int magicNumber)
        {
            _magicNumber = magicNumber;
        }

        public int Handle(Unit unit) => _magicNumber;
    }

    [Fact]
    public void ShouldProcessTransientHandler()
    {
        var handlerFactory = new TestHandlerFactory();

        var transientHandler = new TransientHandler<Unit, int>(typeof(TestHandler), handlerFactory);
        var result = transientHandler.Handle(Unit.Value);

        Assert.Equal(42, result);
    }
}
