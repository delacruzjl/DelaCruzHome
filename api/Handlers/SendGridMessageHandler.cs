using Api.Interfaces;
using Api.Models;
using SendGrid.Helpers.Mail;

namespace Api.Handlers;

public class SendGridMessageHandler : ISendGridMessageHandler
{
    private readonly SendGridConfiguration _sendGridConfiguration;

    public SendGridMessageHandler(SendGridConfiguration sendGridConfiguration)
    {
        _sendGridConfiguration = sendGridConfiguration;
    }

    public SendGridMessage MakeSendGridMessage(MessageForm contact)
    {

        var msg = new SendGridMessage();
        msg.SetTemplateId(_sendGridConfiguration.TemplateId);
        msg.SetFrom(_sendGridConfiguration.EmailAddress, _sendGridConfiguration.SenderName);
        msg.AddTo(_sendGridConfiguration.WebsiteAdminEmail);

        // msg.AddBcc(contact.Email, contact.FullName);
        msg.SetSubject(_sendGridConfiguration.SubjectLine);

        msg.SetTemplateData(new
        {
            fullName = contact.FullName,
            email = contact.Email,
            content = contact.Content,
            subject = _sendGridConfiguration.SubjectLine
        });

        // disable tracking settings
        // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        msg.SetOpenTracking(false);
        msg.SetGoogleAnalytics(false);
        msg.SetSubscriptionTracking(true);

        return msg;
    }
}