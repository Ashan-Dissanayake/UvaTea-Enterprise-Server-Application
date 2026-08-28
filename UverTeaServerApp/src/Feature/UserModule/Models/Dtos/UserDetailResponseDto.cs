namespace UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

public record UserDetailResponseDto(
    int Id,
    string? Username,
    UserStatusDto Userstatus,
    int EmployeeId,
    RoleDto Role
);
