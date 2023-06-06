using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Api.Models;
using SendGrid;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api;

public class NewsletterSubscriber
{
    private readonly ISendGridClient _sendGridClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public NewsletterSubscriber(
        ISendGridClient sendGridClient,
        JsonSerializerOptions jsonOptions)
    {
        _sendGridClient = sendGridClient;
        _jsonOptions = jsonOptions;
    }

    [FunctionName("NewsletterSubscriber")]
    public async Task<IActionResult> Subscribe(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger _logger)
    {
        using (_logger.BeginScope("NewsletterSubscriber"))
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var contact = JsonSerializer.Deserialize<NewsletterContact>(requestBody, _jsonOptions);

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(contact, new ValidationContext(contact, null, null), results, true);
            if (!isValid)
                throw new Exception(string.Join(", ", results.Select(r => r.ErrorMessage)));

            var data = JsonSerializer.Serialize(
                new
                {
                    list_ids = new[] { Environment.GetEnvironmentVariable("SENDGRID_NEWSLETTER_LIST_ID") },
                    Contacts = new[] { contact }
                }, _jsonOptions);
            _logger.LogInformation($"SendGrid.RequestBody: {data}");

            // add recipient
            var response = await _sendGridClient.RequestAsync(
                method: SendGridClient.Method.PUT,
                urlPath: "marketing/contacts",
                requestBody: data
            );

            var message = response.Body.ReadAsStringAsync().Result;
            if ((int)response.StatusCode > StatusCodes.Status400BadRequest)
                throw new Exception(message);

            _logger.LogInformation($"SendGrid.Response: {message}");
            return new StatusCodeResult((int)response.StatusCode);
        }
    }
}

