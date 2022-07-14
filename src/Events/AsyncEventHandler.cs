namespace Kantaiko.Routing.Events;

public delegate Task AsyncEventHandler<in TContext>(TContext context);
