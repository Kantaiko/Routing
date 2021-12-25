using Xunit;

namespace Kantaiko.Routing.Tests;

public class RouterHandlerTest
{
    private interface IRequest { }

    private record MultipleRequest(int A, int B) : IRequest;

    private record SumRequest(int A, int B) : IRequest;

    [Fact]
    public void ShouldRouteRequestByType()
    {
        var routerHandler = Handler.Router(new Dictionary<Type, IHandler<IRequest, int>>
        {
            [typeof(MultipleRequest)] = Handler.Function<MultipleRequest, IRequest, int>(input => input.A * input.B),
            [typeof(SumRequest)] = Handler.Function<SumRequest, IRequest, int>(input => input.A + input.B)
        });

        Assert.Equal(24, routerHandler.Handle(new MultipleRequest(12, 2)));
        Assert.Equal(42, routerHandler.Handle(new SumRequest(21, 21)));
    }
}
