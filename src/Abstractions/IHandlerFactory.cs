namespace Kantaiko.Routing.Abstractions;

public interface IHandlerFactory
{
    object CreateHandler(Type handlerType, IServiceProvider serviceProvider);
}
