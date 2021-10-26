namespace Kantaiko.Routing;

public class DefaultServiceProvider : IServiceProvider
{
    public object GetService(Type serviceType)
    {
        throw new InvalidOperationException("DI container is not configured");
    }

    private static DefaultServiceProvider? _instance;
    public static DefaultServiceProvider Instance => _instance ??= new DefaultServiceProvider();
}
