using System;
using FluentValidation;
using HRPayroll.Application.DTOs;

namespace HRPayroll.Application.Validators;

/// <summary>
/// Validator rules for CreateEmployeeDto.
/// </summary>
public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(e => e.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.");

        RuleFor(e => e.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(dob => dob <= DateTime.Today.AddYears(-18))
            .WithMessage("Employee must be at least 18 years old.");

        RuleFor(e => e.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .MaximumLength(10).WithMessage("Gender cannot exceed 10 characters.");

        RuleFor(e => e.DateOfJoining)
            .NotEmpty().WithMessage("Date of joining is required.");

        RuleFor(e => e.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than zero.");

        RuleFor(e => e.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");

        RuleFor(e => e.DesignationId)
            .NotEmpty().WithMessage("Designation ID is required.");

        RuleFor(e => e.Address)
            .NotNull().WithMessage("Address details are required.")
            .SetValidator(new AddressDtoValidator());
    }
}
