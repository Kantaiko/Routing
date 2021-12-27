namespace Kantaiko.Routing.Requests;

public interface IRequest : IRequest<Unit> { }

public interface IRequest<TResponse> { }
