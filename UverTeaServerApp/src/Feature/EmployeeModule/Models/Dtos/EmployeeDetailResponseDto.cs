namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

public record EmployeeDetailResponseDto(
int Id,
string? Number,
string? Fullname,
string? Callingname,
GenderDto Gender,
DateOnly? Dobirth,
string? Nic,
string? Address,
string? Mobile,
string? Land,
DateOnly? Doassignment,
DesignationDto Designation,
EmployeeStatusDto Employeestatus,
string? Description
);
