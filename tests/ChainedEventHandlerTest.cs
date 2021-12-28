using System.Reflection;
using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class ChainedEventHandlerTest
{
    [Fact]
    public async Task ShouldHandleEventUsingChainedEventHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var handler = EventHandlerFactory.CreateChainedEventHandler<EventBase>(types);

        var eventA = new EventA();
        await handler.Handle(new EventContext<EventA>(eventA));

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        await handler.Handle(new EventContext<EventB>(eventB));

        Assert.Equal(42, eventB.Count);
    }

    private class EventBase
    {
        public int Count { get; set; }
    }

    private class EventA : EventBase { }

    private class EventB : EventBase { }

    private class EventHandlerA : ChainedEventHandler<EventA>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventA> context, NextHandler next)
        {
            Event.Count += 23;
            return next();
        }
    }

    private class EventHandlerB : ChainedEventHandler<EventB>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventB> context, NextHandler next)
        {
            Event.Count += 41;
            return next();
        }
    }

    private class GenericEventHandler : ChainedEventHandler<EventBase>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventBase> context, NextHandler next)
        {
            Event.Count++;
            return next();
        }
    }
}
