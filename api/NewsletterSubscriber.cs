using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Jodelac.SendGridAzFunction.Interfaces;
using Jodelac.SendGridAzFunction.Models;

namespace Api;

public class NewsletterSubscriber
{
    private readonly ISubscriptionHelper _subscriptionHelper;
    public NewsletterSubscriber(ISubscriptionHelper subscriptionHelper)
    {
       _subscriptionHelper = subscriptionHelper;
    }

    [FunctionName("NewsletterSubscriber")]
    [OpenApiOperation(operationId: "Subscribe", tags: new[] { "Subscribe" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]        
    [OpenApiRequestBody(contentType: "application/json; charset=utf-8", bodyType: typeof(NewsletterContact), Description = "Sample Newsletter Contact", Required = true)]
    [OpenApiResponseWithoutBody(statusCode: System.Net.HttpStatusCode.Accepted, Description = "The response was accepted by SendGrid API")]
    public async Task<IActionResult> Subscribe(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger _logger)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        int response = await _subscriptionHelper.SubscribeContactToSite(req.Body, _logger);
        return new StatusCodeResult(response);
    }
    
}

