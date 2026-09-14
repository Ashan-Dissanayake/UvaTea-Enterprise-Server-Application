using System.Security.Claims;

namespace UverTeaServerApp.Shared.Security;

public interface ICurrentUser
{
    int UserId { get; }

    string? Username { get; }

    bool IsAuthenticated { get; }

    bool IsInRole(string role);
}