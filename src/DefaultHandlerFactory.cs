using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing;

public class DefaultHandlerFactory : IHandlerFactory
{
    public object CreateHandler(Type handlerType, IServiceProvider serviceProvider)
    {
        return Activator.CreateInstance(handlerType)!;
    }

    private static DefaultHandlerFactory? _instance;
    public static DefaultHandlerFactory Instance => _instance ??= new DefaultHandlerFactory();
}
