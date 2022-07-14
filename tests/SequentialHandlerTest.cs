using Xunit;

namespace Kantaiko.Routing.Tests;

public class SequentialHandlerTest
{
    private class TestContext
    {
        public int Count { get; set; }
    }

    [Fact]
    public void ShouldRunAsyncHandlersSequentially()
    {
        var handler = Handler.ParallelAsync(new[]
        {
            Handler.Function<TestContext, Task<Unit>>(context =>
            {
                Assert.Equal(0, context.Count++);

                return Unit.Task;
            }),
            Handler.Function<TestContext, Task<Unit>>(context =>
            {
                Assert.Equal(1, context.Count++);

                return Unit.Task;
            })
        });

        var context = new TestContext();
        handler.Handle(context);

        Assert.Equal(2, context.Count);
    }
}
