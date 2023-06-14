using System;
using Jodelac.SendGridAzFunction.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SendGrid;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;
public class Startup : FunctionsStartup
// END: ed8c6549bwf9
{
    public IConfiguration Configuration { get; private set; }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        string cs = Environment.GetEnvironmentVariable("connectionString");
        var environment = Environment.GetEnvironmentVariable("environment");
        builder.ConfigurationBuilder.AddAzureAppConfiguration(options => {
            options.Connect(cs)
                .Select(KeyFilter.Any, LabelFilter.Null);
            
            if (!string.IsNullOrWhiteSpace(environment)){
                options.Select(KeyFilter.Any, environment);
            }                
        });
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        Configuration = builder.GetContext().Configuration;
        var services = builder.Services;
        services.AddLogging();
        services.AddSendGridAzFunction(Configuration);
       
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