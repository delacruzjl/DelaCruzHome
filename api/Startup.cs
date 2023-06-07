using System;
using System.Text.Json;
using Api.Models;
using Api.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SendGrid;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;
public class Startup : FunctionsStartup
// END: ed8c6549bwf9
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        services.AddLogging();
        services.AddSingleton(new SendGridConfiguration());
        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });
        services.AddValidatorsFromAssemblyContaining<NewsletterContactValidator>();
        
        var apiKey = SendGridConfiguration.ApiKey;
        services.AddSingleton<ISendGridClient>(new SendGridClient(apiKey));

        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                            {
                                var options = new OpenApiConfigurationOptions()
                                {
                                    Info = new OpenApiInfo()
                                    {
                                        Version = "1.0.0",
                                        Title = "Swagger Jose's SendGrid Newsletter Subscription API",
                                        Description = "API designed by [http://delacruzhome.com](http://delacruzhome.com).",
                                        TermsOfService = new Uri("https://github.com/delacruzjl"),
                                        Contact = new OpenApiContact()
                                        {
                                            Name = "Jose",
                                            Email = Environment.GetEnvironmentVariable("SupportEmail"),
                                            Url = new Uri("https://github.com/delacruzjl"),
                                        },
                                        License = new OpenApiLicense()
                                        {
                                            Name = "MIT",
                                            Url = new Uri("http://opensource.org/licenses/MIT"),
                                        }
                                    },
                                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                                    OpenApiVersion = OpenApiVersionType.V2,
                                    IncludeRequestingHostName = true,
                                    ForceHttps = false,
                                    ForceHttp = false,
                                };

                                return options;
                            });
    }
}