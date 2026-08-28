
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.src.Shared.Security;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}