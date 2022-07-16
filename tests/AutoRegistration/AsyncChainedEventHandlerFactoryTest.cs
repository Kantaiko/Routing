using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;
using Kantaiko.Routing.Tests.AutoRegistration.Shared;
using Xunit;

namespace Kantaiko.Routing.Tests.AutoRegistration;

public class AsyncChainedEventHandlerFactoryTest
{
    [Fact]
    public async Task ShouldHandlePolymorphicEvent()
    {
        var types = TestUtils.CreateLookupTypes<AsyncChainedEventHandlerFactoryTest>(typeof(AsyncEventContext<>));

        var handler = EventHandlerFactory.ChainedAsync<IAsyncEventContext<EventBase>>(types);

        var eventA = new EventA();
        await handler.Handle(new AsyncEventContext<EventA>(eventA), () => Unit.Task);

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        await handler.Handle(new AsyncEventContext<EventB>(eventB), () => Unit.Task);

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public async Task ShouldHandleStaticEvent()
    {
        var types = TestUtils.CreateLookupTypes<AsyncChainedEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.ChainedAsync<IAsyncEventContext<EventA>>(types);

        var eventA = new EventA();
        await handler.Handle(new AsyncEventContext<EventA>(eventA), () => Unit.Task);

        Assert.Equal(24, eventA.Count);
    }

    [Fact]
    public async Task ShouldHandleNonGenericEventContext()
    {
        var types = TestUtils.CreateLookupTypes<AsyncChainedEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.ChainedAsync<TestEventContext>(types);

        var context = new TestEventContext();
        await handler.Handle(context, () => Unit.Task);

        Assert.Equal(42, context.Count);
    }

    private class EventHandlerA : AsyncChainedEventHandlerBase<EventA>
    {
        protected override Task<Unit> HandleAsync(IAsyncEventContext<EventA> context, Func<Task<Unit>> next)
        {
            Event.Count += 23;

            return next();
        }
    }

    private class EventHandlerB : AsyncChainedEventHandlerBase<EventB>
    {
        protected override Task<Unit> HandleAsync(IAsyncEventContext<EventB> context, Func<Task<Unit>> next)
        {
            Event.Count += 41;

            return next();
        }
    }

    private class GenericEventHandler : AsyncChainedEventHandlerBase<EventBase>
    {
        protected override Task<Unit> HandleAsync(IAsyncEventContext<EventBase> context, Func<Task<Unit>> next)
        {
            Event.Count++;

            return next();
        }
    }

    private class TestEventContextHandler : IChainedHandler<TestEventContext, Task<Unit>>, IAutoRegistrableHandler
    {
        public Task<Unit> Handle(TestEventContext input, Func<Task<Unit>> next)
        {
            input.Count = 42;

            return next();
        }
    }
}
