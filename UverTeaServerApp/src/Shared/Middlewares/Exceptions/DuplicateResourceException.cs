
namespace UverTeaServerApp.src.shared.Middlewares.Exceptions;

public class DuplicateResourceException : Exception
{
    public DuplicateResourceException(string message) : base(message) { }
}