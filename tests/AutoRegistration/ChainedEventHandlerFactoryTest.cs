using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;
using Kantaiko.Routing.Tests.AutoRegistration.Shared;
using Xunit;

namespace Kantaiko.Routing.Tests.AutoRegistration;

public class ChainedEventHandlerFactoryTest
{
    [Fact]
    public void ShouldHandlePolymorphicEvent()
    {
        var types = TestUtils.CreateLookupTypes<ChainedEventHandlerFactoryTest>(typeof(EventContext<>));

        var handler = EventHandlerFactory.Chained<IEventContext<EventBase>>(types);

        var eventA = new EventA();
        handler.Handle(new EventContext<EventA>(eventA), () => default);

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        handler.Handle(new EventContext<EventB>(eventB), () => default);

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public void ShouldHandleStaticEvent()
    {
        var types = TestUtils.CreateLookupTypes<ChainedEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.Chained<IEventContext<EventA>>(types);

        var eventA = new EventA();
        handler.Handle(new EventContext<EventA>(eventA), () => default);

        Assert.Equal(24, eventA.Count);
    }

    [Fact]
    public void ShouldHandleNonGenericEventContext()
    {
        var types = TestUtils.CreateLookupTypes<ChainedEventHandlerFactoryTest>();

        var handler = EventHandlerFactory.Chained<TestEventContext>(types);

        var context = new TestEventContext();
        handler.Handle(context, () => default);

        Assert.Equal(42, context.Count);
    }

    private class EventHandlerA : ChainedEventHandlerBase<EventA>
    {
        protected override Unit Handle(IEventContext<EventA> context, Func<Unit> next)
        {
            Event.Count += 23;

            return next();
        }
    }

    private class EventHandlerB : ChainedEventHandlerBase<EventB>
    {
        protected override Unit Handle(IEventContext<EventB> context, Func<Unit> next)
        {
            Event.Count += 41;

            return next();
        }
    }

    private class GenericEventHandler : ChainedEventHandlerBase<EventBase>
    {
        protected override Unit Handle(IEventContext<EventBase> context, Func<Unit> next)
        {
            Event.Count++;

            return next();
        }
    }

    private class TestEventContextHandler : IChainedHandler<TestEventContext, Unit>, IAutoRegistrableHandler
    {
        public Unit Handle(TestEventContext input, Func<Unit> next)
        {
            input.Count = 42;

            return next();
        }
    }
}
