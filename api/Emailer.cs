using System.Net.Mime;
using System.Threading.Tasks;
using Jodelac.SendGridAzFunction.Interfaces;
using Jodelac.SendGridAzFunction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SendGrid.Helpers.Mail;

namespace Api;

public class Emailer {

    private readonly IEmailHelper _emailHelper;
    public Emailer(IEmailHelper emailHelper)
    {
       _emailHelper = emailHelper;
    }

    [FunctionName("Emailer")]
    [OpenApiOperation(operationId: "Send", tags: new[] { "EmailerSend" })]
    [OpenApiSecurity(ApiSwaggerInfo.TestFunctionApiKey, SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = ApiSwaggerInfo.TestFunctionApiHeaderName)]        
    [OpenApiRequestBody(contentType: $"{MediaTypeNames.Application.Json}; charset=utf-8", bodyType: typeof(ContactForm), Description = "Sample SendGrid Message", Required = true)]
    [OpenApiResponseWithoutBody(statusCode: System.Net.HttpStatusCode.NoContent, Description = "The email was submited through SendGrid API")]
    public async Task Send(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, 
        [SendGrid] IAsyncCollector<SendGridMessage> messageCollector,
        ILogger _logger) {
            
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await _emailHelper.QueueEmailToSendGrid(req.Body, messageCollector, _logger);
        
    }
}