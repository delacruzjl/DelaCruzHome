using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Api;

public class Emailer {
    private readonly ISendGridClient _sendGridClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IValidator<MessageForm> _validator;

    public Emailer(
        ISendGridClient sendGridClient,
        JsonSerializerOptions jsonOptions,
        IValidator<MessageForm> validator)
    {
        _sendGridClient = sendGridClient;
        _jsonOptions = jsonOptions;
        _validator = validator;        
    }

    [FunctionName("Emailer")]
    [OpenApiOperation(operationId: "Send", tags: new[] { "EmailerSend" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]        
    [OpenApiRequestBody(contentType: "application/json; charset=utf-8", bodyType: typeof(MessageForm), Description = "Sample SendGrid Message", Required = true)]
    [OpenApiResponseWithoutBody(statusCode: System.Net.HttpStatusCode.NoContent, Description = "The email was submited through SendGrid API")]
    public async Task Send(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, 
        [SendGrid] IAsyncCollector<SendGridMessage> messageCollector,
        ILogger _logger) {
            using (_logger.BeginScope("Emailer")) {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var message = await req.Body.ConvertFrom<MessageForm>(_jsonOptions, _logger);
                await message.Validate(_validator, _logger);

                var names = message.FullName.Split(' ');
                NewsletterContact contact = new(){
                    Email = message.Email,
                    FirstName = names.First(),
                    LastName = names.Last()
                };

                _ = await _sendGridClient.AddContactToSendGridList(contact, _jsonOptions, _logger);
                _ = await _sendGridClient.AddContactToSendGridGroup(contact, _jsonOptions, _logger);
                
                var SendGridMessage = message.ToSendGridMessage();
                await messageCollector.AddAsync(SendGridMessage);
            }
    }
}