
namespace UverTeaServerApp.Shared.Middlewares;

public class DuplicateResourceException : Exception
{
    public DuplicateResourceException(string message) : base(message) { }
}