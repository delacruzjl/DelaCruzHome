using Api.Models;
using FluentValidation;

namespace Api.Validators;

public class NewsletterContactValidator : AbstractValidator<NewsletterContact>
{
    public NewsletterContactValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
