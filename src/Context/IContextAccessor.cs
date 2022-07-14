namespace Kantaiko.Routing.Context;

public interface IContextAccessor
{
    object? Context { get; }
}
