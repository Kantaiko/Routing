using Xunit;

namespace Kantaiko.Routing.Tests;

public class ParallelHandlerTest
{
    private class TestContext
    {
        public int Count { get; set; }
    }

    [Fact]
    public void ShouldRunAsyncHandlersParallel()
    {
        var handler = Handler.SequentialAsync(new[]
        {
            Handler.Function<TestContext, Task<Unit>>(context =>
            {
                lock (context)
                {
                    context.Count++;
                }

                return Unit.Task;
            }),
            Handler.Function<TestContext, Task<Unit>>(context =>
            {
                lock (context)
                {
                    context.Count++;
                }

                return Unit.Task;
            })
        });

        var context = new TestContext();
        handler.Handle(context);

        Assert.Equal(2, context.Count);
    }
}
