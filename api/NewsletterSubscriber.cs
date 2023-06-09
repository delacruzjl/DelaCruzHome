using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Api.Models;
using SendGrid;
using FluentValidation;
using Api.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Api.Handlers;
using Api.Interfaces;

namespace Api;

public class NewsletterSubscriber
{
    private readonly ISendGridClient _sendGridClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IValidator<NewsletterContact> _validator;
    private readonly ISendGridContactHandler _sendGridContactHandler;
    private readonly SendGridConfiguration _config;

    public NewsletterSubscriber(
        ISendGridClient sendGridClient,
        SendGridConfiguration config,
        JsonSerializerOptions jsonOptions,
        IValidator<NewsletterContact> validator,
        ISendGridContactHandler sendGridContactHandler)
    {
        _sendGridClient = sendGridClient;
        _config = config;
        _jsonOptions = jsonOptions;
        _validator = validator;
        _sendGridContactHandler = sendGridContactHandler;
    }

    [FunctionName("NewsletterSubscriber")]
    [OpenApiOperation(operationId: "Subscribe", tags: new[] { "Subscribe" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]        
    [OpenApiRequestBody(contentType: "application/json; charset=utf-8", bodyType: typeof(NewsletterContact), Description = "Sample Newsletter Contact", Required = true)]
    [OpenApiResponseWithoutBody(statusCode: System.Net.HttpStatusCode.Accepted, Description = "The response was accepted by SendGrid API")]
    public async Task<IActionResult> Subscribe(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger _logger)
    {
        using (_logger.BeginScope("NewsletterSubscriber"))
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var contact = await req.Body.ConvertFrom<NewsletterContact>(_jsonOptions, _logger);
            await contact.Validate(_validator, _logger);
            _ = await _sendGridContactHandler.AddContactToSendGridList(contact, _logger);
            var response = await _sendGridContactHandler.AddContactToSendGridGroup(contact,  _logger);

            return new StatusCodeResult((int)response);
        }
    }
}

