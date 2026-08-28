namespace UverTeaServerApp.src.shared.Middlewares.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message) { }
}