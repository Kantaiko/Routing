using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class EventHandlerTest
{
    [Fact]
    public async Task ShouldHandlePolymorphicEventUsingEventHandler()
    {
        var types = typeof(EventHandlerTest).GetNestedTypes().Concat(new[] { typeof(EventContext<>) });

        var handler = EventHandlerFactory.CreateSequentialEventHandler<IEventContext<EventBase>>(types);

        var eventA = new EventA();
        await handler.Handle(new EventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        await handler.Handle(new EventContext<EventB>(eventB));

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public async Task ShouldHandleStaticEventUsingEventHandler()
    {
        var types = typeof(EventHandlerTest).GetNestedTypes();

        var handler = EventHandlerFactory.CreateSequentialEventHandler<IEventContext<EventA>>(types);

        var eventA = new EventA();
        await handler.Handle(new EventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);
    }

    [Fact]
    public async Task ShouldHandleNonGenericEventContext()
    {
        var types = typeof(EventHandlerTest).GetNestedTypes();

        var handler = EventHandlerFactory.CreateSequentialEventHandler<TestEventContext>(types);

        var context = new TestEventContext();
        await handler.Handle(context);

        Assert.Equal(42, context.Count);
    }

    public class EventBase
    {
        public int Count { get; set; }
    }

    public class EventA : EventBase { }

    public class EventB : EventBase { }

    public class EventHandlerA : EventHandlerBase<EventA>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventA> context)
        {
            Event.Count += 23;
            return Unit.Task;
        }
    }

    public class EventHandlerB : EventHandlerBase<EventB>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventB> context)
        {
            Event.Count += 41;
            return Unit.Task;
        }
    }

    public class GenericEventHandlerBase : EventHandlerBase<EventBase>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventBase> context)
        {
            Event.Count++;
            return Unit.Task;
        }
    }

    public class TestEventContext
    {
        public int Count { get; set; }
    }

    public class TestEventContextHandler : IHandler<TestEventContext, Task>, IAutoRegistrableHandler
    {
        public Task Handle(TestEventContext input)
        {
            input.Count = 42;

            return Task.CompletedTask;
        }
    }
}
