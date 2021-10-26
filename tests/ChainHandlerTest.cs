using Xunit;

namespace Kantaiko.Routing.Tests;

public class ChainHandlerTest
{
    [Fact]
    public void ShouldProcessChainHandler()
    {
        var chainHandler = Handler.Chain(new[]
        {
            Handler.Function<int, int>((x, next) => next(x + 2)),
            Handler.Function<int, int>((x, next) => next(x * 2)),
            Handler.Function<int, int>(x => x)
        });

        Assert.Equal(4, chainHandler.Handle(0));
        Assert.Equal(8, chainHandler.Handle(2));
    }

    [Fact]
    public async Task ShouldProcessAsyncChainHandler()
    {
        var chainHandler = Handler.Chain(new[]
        {
            Handler.Function<int, Task<int>>((x, next) => next(x + 2)),
            Handler.Function<int, Task<int>>((x, next) => next(x * 2)),
            Handler.Function<int, Task<int>>(Task.FromResult)
        });

        Assert.Equal(4, await chainHandler.Handle(0));
        Assert.Equal(8, await chainHandler.Handle(2));
    }
}
