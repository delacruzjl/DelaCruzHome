using Api.Models;
using FluentValidation;

namespace Api.Validator;

public class SendGridMessageValidator : AbstractValidator<MessageForm> {
    public SendGridMessageValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required");
        
        RuleFor(x => x.Email)
        .NotEmpty()
        .EmailAddress();

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Message content is required");
    }
}