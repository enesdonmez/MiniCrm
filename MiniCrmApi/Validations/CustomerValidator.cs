using FluentValidation;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Validations;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required")
            .MaximumLength(100).WithMessage("Full Name must not exceed 100 characters")
            .MinimumLength(4).WithMessage("Full Name must not exceed 4 characters");
        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Company Name is required")
            .MaximumLength(250).WithMessage("Company Name must not exceed 250 characters");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters")
            .MinimumLength(10).WithMessage("Phone must not exceed 10 characters");
    }
}
