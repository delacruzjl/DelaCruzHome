using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api;
 public class Startup : FunctionsStartup
    // END: ed8c6549bwf9
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // builder.Services.AddScoped<IMyService, MyService>();
        }

    }