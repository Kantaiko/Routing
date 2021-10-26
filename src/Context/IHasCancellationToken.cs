namespace Kantaiko.Routing.Context;

public interface IHasCancellationToken
{
    CancellationToken CancellationToken { get; }
}
