using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    
    [Required(ErrorMessage = "Employee number is required.")]
    [RegularExpression("^\\[E]d{3}$", ErrorMessage = "Invalid Number")]
    string Number,

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
    string Fullname,

    [RegularExpression("^([A-Z][a-z]+)$", ErrorMessage = "Invalid Calling Name")]
    string? Callingname,

    [Required(ErrorMessage = "NIC is required.")]
    [RegularExpression(@"^([0-9]{9}[vVxX]|[0-9]{12})$", ErrorMessage = "Invalid NIC format.")]
    string Nic,

    [Required(ErrorMessage = "Mobile number is required.")]
    [RegularExpression(@"^(?:0|94|\+94)?(7[01245678]\d{7})$", ErrorMessage = "Invalid Sri Lankan mobile number format.")]
    string? Mobile,

    [RegularExpression("^\\d{0,10}$", ErrorMessage = "Invalid Land Number")]
    string? Land,

    [Required(ErrorMessage = "Address is required.")]
    [RegularExpression("^([\\w\\/\\-,\\s]{2,})$", ErrorMessage = "Invalid Address")]
    string? Address,

    DateOnly? Dobirth,

    DateOnly? Doassignment,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Gender ID is required.")]
    int GenderId,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Designation ID is required.")]
    int DesignationId,

    [Range(1, int.MaxValue, ErrorMessage = "A valid Employee Status ID is required.")]
    int EmployeestatusId,

    [RegularExpression("^.*$", ErrorMessage = "Invalid Description")]
    string? Description

) : IRequest<EmployeeDetailResponseDto>;