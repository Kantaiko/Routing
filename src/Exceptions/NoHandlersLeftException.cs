namespace Kantaiko.Routing.Exceptions;

public class NoHandlersLeftException : Exception
{
    public NoHandlersLeftException() : base("No handlers left") { }
}
