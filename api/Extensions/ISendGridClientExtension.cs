using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendGrid;

namespace Api.Extensions;

public static class ISendGridClientExtension {
    public static async Task<int> AddContactToSendGridGroup(this ISendGridClient sendGridClient, NewsletterContact contact, JsonSerializerOptions jsonOptions, ILogger logger) {
        using (logger.BeginScope("NewsletterContactExtension.AddContactToGroup")){
            logger.LogInformation("Adding contact to group");

            var groupId = SendGridConfiguration.SuppressionGroupId;
            var data = new {
                recipient_emails = new[]{ contact.Email }
            };

            var response = await sendGridClient.RequestAsync(
                method: SendGridClient.Method.POST,
                urlPath: $"asm/groups/{groupId}/suppressions",
                requestBody: JsonSerializer.Serialize(data, jsonOptions)
            );

            Activity.Current?.AddBaggage("SendGrid.response.StatusCode", response.StatusCode.ToString());

            var message = await response.Body.ReadAsStringAsync();
            if ((int)response.StatusCode >= StatusCodes.Status400BadRequest) {
                logger.LogError($"SendGrid.Response: {message}");
                throw new Exception(message);
            }
 
            Activity.Current?.AddBaggage("SendGrid.message", message);
            return (int)response.StatusCode;
        }
    }

    public static async Task<int> AddContactToSendGridList(this ISendGridClient sendGridClient, NewsletterContact contact, JsonSerializerOptions jsonOptions, ILogger logger) {
        using (logger.BeginScope("NewsletterContactExtension.AddContactToSendGrid")) {
            logger.LogInformation("Adding contact to SendGrid");

             var data = JsonSerializer.Serialize(
                new
                {
                    list_ids = new[] { SendGridConfiguration.NewsletterListId },
                    Contacts = new[] { contact }
                }, jsonOptions);
            Activity.Current?.AddBaggage("SendGrid.RequestBody", data);

            // add recipient
            var response = await sendGridClient.RequestAsync(
                method: SendGridClient.Method.PUT,
                urlPath: "marketing/contacts",
                requestBody: data
            );
            Activity.Current?.AddBaggage("SendGrid.response.StatusCode", response.StatusCode.ToString());

            var message = await response.Body.ReadAsStringAsync();
            if ((int)response.StatusCode >= StatusCodes.Status400BadRequest) {
                logger.LogError($"SendGrid.Response: {message}");
                throw new Exception(message);
            }
 
            Activity.Current?.AddBaggage("SendGrid.message", message);
            return (int)response.StatusCode;
        }
    }
}