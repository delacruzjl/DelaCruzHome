using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Api.ConfigureOptions;

public class SwaggerInfoOptionSetup : IConfigureOptions<ApiSwaggerInfo>
{
    private readonly IConfiguration _configuration;
    private readonly IValidator<ApiSwaggerInfo> _swaggerInfoValidator;

    public SwaggerInfoOptionSetup(IConfiguration configuration, IValidator<ApiSwaggerInfo> swaggerInfoValidator)
    {
        _configuration = configuration;
        _swaggerInfoValidator = swaggerInfoValidator;
    }

    public void Configure(ApiSwaggerInfo options)
    {
        _configuration.GetSection(nameof(ApiSwaggerInfo)).Bind(options);
        _swaggerInfoValidator.ValidateAndThrow(options);
    }
}