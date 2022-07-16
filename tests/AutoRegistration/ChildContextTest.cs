using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Context;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Tests.AutoRegistration.Shared;
using Xunit;

namespace Kantaiko.Routing.Tests.AutoRegistration;

public class ChildContextTest
{
    [Fact]
    public void ShouldHandlePolymorphicEventWithChildContext()
    {
        var types = TestUtils.CreateLookupTypes<ChildContextTest>(typeof(EventContext<>));

        var handler = EventHandlerFactory.Chained<IEventContext<EventBase>>(types);

        var eventA = new EventA();
        var context = new ChildEventContext<EventA>(eventA);

        handler.Handle(context, () => default);

        Assert.Equal(42, eventA.Count);
    }

    private class EventHandlerA : ChainedEventHandlerBase<EventA>
    {
        protected override Unit Handle(IEventContext<EventA> context, Func<Unit> next)
        {
            Event.Count += 42;

            return next();
        }
    }

    private interface IChildEventContext<out TEvent> : IEventContext<TEvent> { }

    private class ChildEventContext<TEvent> : ContextBase, IChildEventContext<TEvent> where TEvent : EventA
    {
        public ChildEventContext(TEvent @event, IServiceProvider? serviceProvider = null) : base(serviceProvider)
        {
            Event = @event;
        }

        public TEvent Event { get; }
    }
}
