
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.Shared.Security;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
