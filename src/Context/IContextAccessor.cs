namespace Kantaiko.Routing.Context;

public interface IContextAccessor<out TContext> : IContextAccessor where TContext : IContext
{
    new TContext? Context { get; }

    IContext? IContextAccessor.Context => Context;
}

public interface IContextAccessor
{
    IContext? Context { get; }
}
