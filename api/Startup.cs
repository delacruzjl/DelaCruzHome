using System;
using System.Text.Json;
using Api.Handlers;
using Api.Interfaces;
using Api.Models;
using Api.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SendGrid;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;
public class Startup : FunctionsStartup
// END: ed8c6549bwf9
{
    private IConfiguration _configuration;
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("connectionString");
        var label = Environment.GetEnvironmentVariable("label") ?? "";

        builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>{
            options.Connect(connectionString);
            options.Select(KeyFilter.Any, label);
        });

        _configuration = builder.ConfigurationBuilder.Build();
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;
        services.AddAzureAppConfiguration();
        services.AddSingleton<IConfiguration>(_configuration);
        
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>(); 
        SendGridConfiguration sengridConfiguration = new(configuration);
        services.AddSingleton(sengridConfiguration);

        services.AddLogging();
        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });
        services.AddValidatorsFromAssemblyContaining<NewsletterContactValidator>();
        
        var apiKey = sengridConfiguration.ApiKey;
        services.AddSingleton<ISendGridClient>(new SendGridClient(apiKey));
        services.AddSingleton(new SendGridMessageHandler(sengridConfiguration));
        services.AddScoped<ISendGridMessageHandler, SendGridMessageHandler>();
        services.AddScoped<ISendGridContactHandler, SendGridContactHandler>();

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