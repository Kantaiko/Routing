namespace Kantaiko.Routing;

public struct Unit
{
    public static Unit Value => default;

    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Value);
    public static ValueTask<Unit> ValueTask => System.Threading.Tasks.ValueTask.FromResult(default(Unit));
}
