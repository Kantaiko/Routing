using Kantaiko.Routing.Events;
using Xunit;

namespace Kantaiko.Routing.Tests;

public class AsyncEventHandlerTest
{
    private class TestEventContext
    {
        public List<int> Numbers { get; } = new();
    }

    private class TestEventContainer
    {
        public event AsyncEventHandler<TestEventContext>? TestEventHappened;

        public async Task<TestEventContext> InvokeSequentially()
        {
            var context = new TestEventContext();

            await TestEventHappened.InvokeAsync(context);

            return context;
        }

        public async Task<TestEventContext> InvokeParallel()
        {
            var context = new TestEventContext();

            await TestEventHappened.InvokeParallelAsync(context);

            return context;
        }
    }

    [Fact]
    public async Task ShouldHandleEventsSequentially()
    {
        var container = new TestEventContainer();

        container.TestEventHappened += async context =>
        {
            await Task.Delay(1);

            context.Numbers.Add(1);
        };

        container.TestEventHappened += context =>
        {
            context.Numbers.Add(2);

            return Task.CompletedTask;
        };

        var context = await container.InvokeSequentially();

        Assert.Equal(new List<int> { 1, 2 }, context.Numbers);
    }

    [Fact]
    public async Task ShouldHandleEventsParallel()
    {
        var container = new TestEventContainer();

        container.TestEventHappened += async context =>
        {
            await Task.Delay(1);

            context.Numbers.Add(1);
        };

        container.TestEventHappened += context =>
        {
            context.Numbers.Add(2);

            return Task.CompletedTask;
        };

        var context = await container.InvokeParallel();

        Assert.Equal(new List<int> { 2, 1 }, context.Numbers);
    }
}
