using Api.Models;
using SendGrid.Helpers.Mail;

namespace Api.Extensions;

public static class MessageFormExtension {
    public static SendGridMessage ToSendGridMessage(this MessageForm contact) {

        var msg = new SendGridMessage();
        msg.SetTemplateId(SendGridConfiguration.TemplateId);
        msg.SetFrom(SendGridConfiguration.EmailAddress, SendGridConfiguration.SenderName);
        msg.AddTo(SendGridConfiguration.WebsiteAdminEmail);
        
        // msg.AddBcc(contact.Email, contact.FullName);
        msg.SetSubject(SendGridConfiguration.SubjectLine);

        msg.SetTemplateData(new {
          fullName = contact.FullName,
          email = contact.Email,
          content = contact.Content,
          subject = SendGridConfiguration.SubjectLine       
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