using FluentValidation;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Validations;

public class CustomerNoteValidator : AbstractValidator<CustomerNote>
{
    public CustomerNoteValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Note is required")
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters");
    }
}