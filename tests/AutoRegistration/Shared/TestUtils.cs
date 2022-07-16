using System.Reflection;

namespace Kantaiko.Routing.Tests.AutoRegistration.Shared;

public static class TestUtils
{
    public static IEnumerable<Type> CreateLookupTypes<TTest>(params Type[] additionalTypes)
    {
        return typeof(TTest).GetNestedTypes(BindingFlags.NonPublic)
            .Concat(additionalTypes)
            .Concat(new[]
            {
                typeof(EventBase),
                typeof(EventA),
                typeof(EventB)
            });
    }
}
