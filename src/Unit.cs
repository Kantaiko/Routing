using System.Threading.Tasks;

namespace Kantaiko.Routing
{
    public struct Unit
    {
        public static Unit Value => default;
        public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Value);
    }
}
