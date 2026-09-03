using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    
    [Required]
    int Id,

    [Required(ErrorMessage = "Employee number is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Employee number must be between 2 and 50 characters.")]
    string Number,

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
    string Fullname,

    string? Callingname,

    [Required(ErrorMessage = "NIC is required.")]
    [RegularExpression(@"^([0-9]{9}[vVxX]|[0-9]{12})$", ErrorMessage = "Invalid NIC format.")]
    string Nic,

    [Required(ErrorMessage = "Mobile number is required.")]
    [Phone(ErrorMessage = "Invalid mobile number format.")]
    string? Mobile,

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    string? Email,

    string? Land,

    string? Address,

    DateOnly? Dobirth,

    DateOnly? Doassignment,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Gender ID is required.")]
    int GenderId,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Designation ID is required.")]
    int DesignationId,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Employee Status ID is required.")]
    int EmployeestatusId,

    string? Description

) : IRequest<EmployeeDetailResponseDto>, UverTeaServerApp.Shared.Behaviors.ITransactionalRequest;