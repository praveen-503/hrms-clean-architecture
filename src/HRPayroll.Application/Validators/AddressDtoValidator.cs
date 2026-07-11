using FluentValidation;
using HRPayroll.Application.DTOs;

namespace HRPayroll.Application.Validators;

/// <summary>
/// Validator rules for AddressDto.
/// </summary>
public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(150).WithMessage("Street cannot exceed 150 characters.");

        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

        RuleFor(a => a.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State cannot exceed 50 characters.");

        RuleFor(a => a.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");

        RuleFor(a => a.ZipCode)
            .NotEmpty().WithMessage("ZipCode is required.")
            .MaximumLength(10).WithMessage("ZipCode cannot exceed 10 characters.");
    }
}
