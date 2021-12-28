using Kantaiko.Routing.Context;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class ContextServiceProviderTest
{
    [Fact]
    public void ShouldResolveContextFromServiceProvider()
    {
        var createdContext = new ContextBase();
        var resolvedContext = (IContext) createdContext.ServiceProvider.GetService(typeof(IContext))!;

        Assert.Same(createdContext, resolvedContext);
    }
}
