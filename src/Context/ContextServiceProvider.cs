namespace Kantaiko.Routing.Context;

public class ContextServiceProvider : IServiceProvider
{
    private readonly IContext _context;
    private readonly IServiceProvider _original;

    public ContextServiceProvider(IContext context, IServiceProvider original)
    {
        _context = context;
        _original = original;
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType.IsAssignableTo(typeof(IContext)))
        {
            return _context;
        }

        return _original.GetService(serviceType);
    }
}
