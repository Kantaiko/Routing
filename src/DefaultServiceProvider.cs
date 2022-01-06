namespace Kantaiko.Routing;

public class DefaultServiceProvider : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        return null;
    }

    private static DefaultServiceProvider? _instance;
    public static DefaultServiceProvider Instance => _instance ??= new DefaultServiceProvider();
}
