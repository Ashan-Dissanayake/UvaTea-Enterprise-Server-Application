namespace UverTeaServerApp.Shared.Middlewares;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message) { }
}