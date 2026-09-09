
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.Shared.Security;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
