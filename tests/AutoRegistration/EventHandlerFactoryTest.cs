using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;
using Kantaiko.Routing.Tests.AutoRegistration.Shared;
using Xunit;

namespace Kantaiko.Routing.Tests.AutoRegistration;

public class EventHandlerFactoryTest
{
    [Fact]
    public void ShouldHandlePolymorphicEvent()
    {
        var types = TestUtils.CreateLookupTypes<EventHandlerFactoryTest>(typeof(EventContext<>));

        var handler = EventHandlerFactory.Sequential<IEventContext<EventBase>>(types);

        var eventA = new EventA();
        handler.Handle(new EventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        handler.Handle(new EventContext<EventB>(eventB));

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public void ShouldHandleStaticEvent()
    {
        var types = TestUtils.CreateLookupTypes<EventHandlerFactoryTest>();

        var handler = EventHandlerFactory.Sequential<IEventContext<EventA>>(types);

        var eventA = new EventA();
        handler.Handle(new EventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);
    }

    [Fact]
    public void ShouldHandleNonGenericEventContext()
    {
        var types = TestUtils.CreateLookupTypes<EventHandlerFactoryTest>();

        var handler = EventHandlerFactory.Sequential<TestEventContext>(types);

        var context = new TestEventContext();
        handler.Handle(context);

        Assert.Equal(42, context.Count);
    }

    private class EventHandlerA : EventHandlerBase<EventA>
    {
        protected override void Handle(IEventContext<EventA> context)
        {
            Event.Count += 23;
        }
    }

    private class EventHandlerB : EventHandlerBase<EventB>
    {
        protected override void Handle(IEventContext<EventB> context)
        {
            Event.Count += 41;
        }
    }

    private class GenericEventHandler : EventHandlerBase<EventBase>
    {
        protected override void Handle(IEventContext<EventBase> context)
        {
            Event.Count++;
        }
    }

    private class TestEventContextHandler : IHandler<TestEventContext, Unit>, IAutoRegistrableHandler
    {
        public Unit Handle(TestEventContext input)
        {
            input.Count = 42;

            return default;
        }
    }
}
