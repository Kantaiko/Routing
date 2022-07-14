using Kantaiko.Routing.Handlers;
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
            [typeof(MultipleRequest)] = Handler.Function<IRequest, int>(input =>
            {
                var multipleRequest = (MultipleRequest) input;

                return multipleRequest.A * multipleRequest.B;
            }),
            [typeof(SumRequest)] = Handler.Function<IRequest, int>(input =>
            {
                var sumRequest = (SumRequest) input;

                return sumRequest.A + sumRequest.B;
            })
        });

        Assert.Equal(24, routerHandler.Handle(new MultipleRequest(12, 2)));
        Assert.Equal(42, routerHandler.Handle(new SumRequest(21, 21)));
    }
}
