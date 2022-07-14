using Kantaiko.Routing.Handlers;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class AuxiliaryHandlerTest
{
    [Fact]
    public void ShouldPerformNullCheck()
    {
        var uncheckedHandler = Handler.Function<string, string>(x => x.ToLower());
        var checkedHandler = uncheckedHandler.WithNullCheck();

        Assert.Throws<NullReferenceException>(() => uncheckedHandler.Handle(null!));
        Assert.Throws<ArgumentNullException>(() => checkedHandler.Handle(null!));
    }

    [Fact]
    public void ShouldPerformNullCheckInChainedHandler()
    {
        var uncheckedHandler = Handler.Function<string, string>((x, _) => x.ToLower());
        var checkedHandler = uncheckedHandler.WithNullCheck();

        Assert.Throws<NullReferenceException>(() => uncheckedHandler.Handle(null!));
        Assert.Throws<ArgumentNullException>(() => checkedHandler.Handle(null!));
    }
}
