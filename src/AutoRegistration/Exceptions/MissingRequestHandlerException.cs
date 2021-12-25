namespace Kantaiko.Routing.AutoRegistration.Exceptions;

public class MissingRequestHandlerException : Exception
{
    public MissingRequestHandlerException(Type requestType) : base(
        $"Handler for request \"{requestType.Name}\" was not found") { }
}
