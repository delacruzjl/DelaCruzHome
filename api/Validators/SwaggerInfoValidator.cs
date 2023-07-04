using FluentValidation;

namespace Api.Validators;

public class SwaggerInfoValidator : AbstractValidator<ApiSwaggerInfo> {
    public SwaggerInfoValidator()
    {
        RuleFor(x => x.Version).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.TermsOfService).NotEmpty();
        RuleFor(x => x.ApiContact).NotNull();
        RuleFor(x => x.ApiLicense).NotNull();
        RuleFor(x => x.ApiContact.Name).NotEmpty();
        RuleFor(x => x.ApiContact.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ApiContact.Url).NotEmpty();
        RuleFor(x => x.ApiLicense.Name).NotEmpty();
        RuleFor(x => x.ApiLicense.Url).NotEmpty();
    }
}