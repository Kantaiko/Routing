using System.Reflection;
using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Events;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class EventHandlerTest
{
    [Fact]
    public async Task ShouldHandlePolymorphicEventUsingEventHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var handler = EventHandlerFactory.CreateSequentialEventHandler<EventBase>(types);

        var eventA = new EventA();
        await handler.Handle(new EventContext<EventA>(eventA, DefaultServiceProvider.Instance, CancellationToken.None));

        Assert.Equal(24, eventA.Count);

        var eventB = new EventB();
        await handler.Handle(new EventContext<EventB>(eventB, DefaultServiceProvider.Instance, CancellationToken.None));

        Assert.Equal(42, eventB.Count);
    }

    [Fact]
    public async Task ShouldHandleStaticEventUsingEventHandler()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var handler = EventHandlerFactory.CreateSequentialEventHandler<EventA>(types);

        var eventA = new EventA();
        await handler.Handle(new EventContext<EventA>(eventA, DefaultServiceProvider.Instance, CancellationToken.None));

        Assert.Equal(24, eventA.Count);
    }

    private class EventBase
    {
        public int Count { get; set; }
    }

    private class EventA : EventBase { }

    private class EventB : EventBase { }

    private class EventHandlerA : Events.EventHandler<EventA>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventA> context)
        {
            Event.Count += 23;
            return Unit.Task;
        }
    }

    private class EventHandlerB : Events.EventHandler<EventB>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventB> context)
        {
            Event.Count += 41;
            return Unit.Task;
        }
    }

    private class GenericEventHandler : Events.EventHandler<EventBase>
    {
        protected override Task<Unit> HandleAsync(IEventContext<EventBase> context)
        {
            Event.Count++;
            return Unit.Task;
        }
    }
}
