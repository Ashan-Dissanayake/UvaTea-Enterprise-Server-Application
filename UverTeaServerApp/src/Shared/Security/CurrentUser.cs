using System.Security.Claims;

namespace UverTeaServerApp.Shared.Security;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated
        ?? false;

    public int UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
            {
                throw new UnauthorizedAccessException(
                    "Authenticated user ID could not be determined.");
            }

            return id;
        }
    }

    public string? Username =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirstValue(ClaimTypes.Name);

    public bool IsInRole(string role) =>
        _httpContextAccessor.HttpContext?
            .User
            .IsInRole(role)
        ?? false;
}