namespace Kantaiko.Routing.Events;

public interface IHasEvent<out TEvent>
{
    TEvent Event { get; }
}
