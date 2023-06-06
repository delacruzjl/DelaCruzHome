using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });

        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        services.AddSingleton<ISendGridClient>(new SendGridClient(apiKey));
    }
}