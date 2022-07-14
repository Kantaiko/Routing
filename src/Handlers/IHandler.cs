namespace Kantaiko.Routing.Handlers;

public interface IHandler<in TInput, out TOutput>
{
    TOutput Handle(TInput input);
}
