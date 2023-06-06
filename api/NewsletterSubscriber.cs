using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Api.Models;
using SendGrid;
using FluentValidation;
using Api.Extensions;

namespace Api;

public class NewsletterSubscriber
{
    private readonly ISendGridClient _sendGridClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IValidator<NewsletterContact> _validator;
    private readonly SendGridConfiguration _config;

    public NewsletterSubscriber(
        ISendGridClient sendGridClient,
        SendGridConfiguration config,
        JsonSerializerOptions jsonOptions,
        IValidator<NewsletterContact> validator)
    {
        _sendGridClient = sendGridClient;
        _config = config;
        _jsonOptions = jsonOptions;
        _validator = validator;
    }

    [FunctionName("NewsletterSubscriber")]
    public async Task<IActionResult> Subscribe(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger _logger)
    {
        using (_logger.BeginScope("NewsletterSubscriber"))
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var contact = await req.Body.ConvertFrom<NewsletterContact>(_jsonOptions, _logger);
            await contact.Validate(_validator, _logger);
            var response = await _sendGridClient.AddContactToSendGrid(contact, _jsonOptions, _logger);

            return new StatusCodeResult((int)response);
        }
    }
}

