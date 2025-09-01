using FluentValidation;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Validations;

public class DealValidator : AbstractValidator<Deal>
{
    public DealValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer Id is required");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters");
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(100).WithMessage("Status must not exceed 100 characters");
    }
}