using System.Threading.Tasks;
using Kantaiko.Routing;

var handler = Handler.Function<int, Task<Unit>>((x, next) =>
{
    if (x == 2)
    {
        return next(x);
    }

    if (x % 2 == 0)
    {
        return next(x);
    }

    return Unit.Task;
});
