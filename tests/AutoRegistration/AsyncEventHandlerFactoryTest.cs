using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;
using Kantaiko.Routing.Tests.AutoRegistration.Shared;
using Xunit;

namespace Kantaiko.Routing.Tests.AutoRegistration;

public class AsyncEventHandlerFactoryTest
{
    [Fact]
    public async Task ShouldHandlePolymorphicEvent()
    {
        var types = TestUtils.CreateLookupTypes<AsyncEventHandlerFactoryTest>(typeof(AsyncEventContext<>));

        var handler = EventHandlerFactory.SequentialAsync<IAsyncEventContext<EventBase>>(types);

        var eventA = new EventA();
        await handler.Handle(new AsyncEventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        await handler.Handle(new AsyncEventContext<EventB>(eventB));

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public async Task ShouldHandleStaticEvent()
    {
        var types = TestUtils.CreateLookupTypes<AsyncEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.SequentialAsync<IAsyncEventContext<EventA>>(types);

        var eventA = new EventA();
        await handler.Handle(new AsyncEventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);
    }

    [Fact]
    public async Task ShouldHandleNonGenericEventContext()
    {
        var types = TestUtils.CreateLookupTypes<AsyncEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.SequentialAsync<TestEventContext>(types);

        var context = new TestEventContext();
        await handler.Handle(context);

        Assert.Equal(42, context.Count);
    }

    private class EventHandlerA : AsyncEventHandlerBase<EventA>
    {
        protected override Task HandleAsync(IAsyncEventContext<EventA> context)
        {
            Event.Count += 23;

            return Task.CompletedTask;
        }
    }

    private class EventHandlerB : AsyncEventHandlerBase<EventB>
    {
        protected override Task HandleAsync(IAsyncEventContext<EventB> context)
        {
            Event.Count += 41;

            return Task.CompletedTask;
        }
    }

    private class GenericEventHandler : AsyncEventHandlerBase<EventBase>
    {
        protected override Task HandleAsync(IAsyncEventContext<EventBase> context)
        {
            Event.Count++;

            return Task.CompletedTask;
        }
    }

    private class TestEventContextHandler : IHandler<TestEventContext, Task>, IAutoRegistrableHandler
    {
        public Task Handle(TestEventContext input)
        {
            input.Count = 42;

            return Task.CompletedTask;
        }
    }
}
