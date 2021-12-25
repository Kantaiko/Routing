using Xunit;

namespace Kantaiko.Routing.Tests;

public class AuxiliaryHandlerTest
{
    [Fact]
    public void ShouldProcessSplitHandler()
    {
        var squareHandler = Handler.Function<int, int>(x => x * x);
        var cubeHandler = Handler.Function<int, int>(x => squareHandler.Handle(x) * x);

        var splitHandler = Handler.Split(x => x % 2 == 0, squareHandler, cubeHandler);

        Assert.Equal(100, splitHandler.Handle(10));
        Assert.Equal(27, splitHandler.Handle(3));
    }

    [Fact]
    public void ShouldWrapHandler()
    {
        var originalHandler = Handler.Function<int, int>(x => x * 2);
        var wrappedHandler = originalHandler.Wrap((x, original) => original(x) + x);

        Assert.Equal(36, wrappedHandler.Handle(12));
    }

    [Fact]
    public void ShouldPerformNullCheck()
    {
        var uncheckedHandler = Handler.Function<string, string>(x => x.ToLower());
        var checkedHandler = uncheckedHandler.CheckNull();

        Assert.Throws<NullReferenceException>(() => uncheckedHandler.Handle(null!));
        Assert.Throws<ArgumentNullException>(() => checkedHandler.Handle(null!));
    }
}
