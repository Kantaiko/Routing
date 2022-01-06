using Kantaiko.Routing.Context;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class ContextAccessorTest
{
    [Fact]
    public void ShouldResolveContextFromServiceProvider()
    {
        var serviceProvider = new TestServiceProvider();

        var createdContext = new ContextBase(serviceProvider);

        var contextAccessor = (IContextAccessor) createdContext.ServiceProvider.GetService(typeof(IContextAccessor))!;

        var baseContextAccessor = (IContextAccessor<ContextBase>) createdContext.ServiceProvider
            .GetService(typeof(IContextAccessor<ContextBase>))!;

        Assert.Same(createdContext, contextAccessor.Context);
        Assert.Same(createdContext, baseContextAccessor.Context);
    }

    private class TestServiceProvider : IServiceProvider
    {
        private readonly ContextAccessor _contextAccessor = new();

        public object GetService(Type serviceType)
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IContextAccessor<>))
            {
                var accessorType = typeof(ContextAccessor<>).MakeGenericType(serviceType.GetGenericArguments()[0]);
                return Activator.CreateInstance(accessorType, _contextAccessor)!;
            }

            if (serviceType == typeof(IContextAcceptor) || serviceType == typeof(IContextAccessor))
            {
                return _contextAccessor;
            }

            throw new InvalidOperationException();
        }
    }
}
