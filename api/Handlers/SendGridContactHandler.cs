using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendGrid;

namespace Api.Handlers;

public class SendGridContactHandler : ISendGridContactHandler
{
    private readonly SendGridConfiguration _sendGridConfiguration;
    private readonly ISendGridClient _sendGridClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public SendGridContactHandler(SendGridConfiguration sendGridConfiguration, ISendGridClient sendGridClient, JsonSerializerOptions jsonOptions)
    {
        _sendGridConfiguration = sendGridConfiguration;
        _sendGridClient = sendGridClient;
        _jsonOptions = jsonOptions;
    }

    public async Task<int> AddContactToSendGridGroup(NewsletterContact contact, ILogger logger)
    {
        using (logger.BeginScope("NewsletterContactExtension.AddContactToGroup"))
        {
            logger.LogInformation("Adding contact to group");

            var groupId = _sendGridConfiguration.SuppressionGroupId;
            var data = new
            {
                recipient_emails = new[] { contact.Email }
            };

            var response = await _sendGridClient.RequestAsync(
                method: SendGridClient.Method.POST,
                urlPath: $"asm/groups/{groupId}/suppressions",
                requestBody: JsonSerializer.Serialize(data, _jsonOptions)
            );

            Activity.Current?.AddBaggage("SendGrid.response.StatusCode", response.StatusCode.ToString());

            var message = await response.Body.ReadAsStringAsync();
            if ((int)response.StatusCode >= StatusCodes.Status400BadRequest)
            {
                logger.LogError($"SendGrid.Response: {message}");
                throw new Exception(message);
            }

            Activity.Current?.AddBaggage("SendGrid.message", message);
            return (int)response.StatusCode;
        }
    }

    public async Task<int> AddContactToSendGridList(NewsletterContact contact, ILogger logger)
    {
        using (logger.BeginScope("NewsletterContactExtension.AddContactToSendGrid"))
        {
            logger.LogInformation("Adding contact to SendGrid");

            var data = JsonSerializer.Serialize(
               new
               {
                   list_ids = new[] { _sendGridConfiguration.NewsletterListId },
                   Contacts = new[] { contact }
               }, _jsonOptions);
            Activity.Current?.AddBaggage("SendGrid.RequestBody", data);

            // add recipient
            var response = await _sendGridClient.RequestAsync(
                method: SendGridClient.Method.PUT,
                urlPath: "marketing/contacts",
                requestBody: data
            );
            Activity.Current?.AddBaggage("SendGrid.response.StatusCode", response.StatusCode.ToString());

            var message = await response.Body.ReadAsStringAsync();
            if ((int)response.StatusCode >= StatusCodes.Status400BadRequest)
            {
                logger.LogError($"SendGrid.Response: {message}");
                throw new Exception(message);
            }

            Activity.Current?.AddBaggage("SendGrid.message", message);
            return (int)response.StatusCode;
        }
    }
}