namespace Kantaiko.Routing.Events;

public delegate void SyncEventHandler<in TContext>(TContext context);
