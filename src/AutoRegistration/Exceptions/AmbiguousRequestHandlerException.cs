namespace Kantaiko.Routing.AutoRegistration.Exceptions;

public class AmbiguousRequestHandlerException : Exception
{
    public AmbiguousRequestHandlerException(Type requestType) : base(
        $"Found multiple handlers for request \"{requestType.Name}\"") { }
}
