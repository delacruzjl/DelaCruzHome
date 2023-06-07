using System;

namespace Api.Models;

public class SendGridConfiguration  {
    public static string ApiKey =>
        Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        ?? throw new ArgumentNullException("SENDGRID_API_KEY");

    public static string NewsletterListId =>
        Environment.GetEnvironmentVariable("SENDGRID_NEWSLETTER_LIST_ID")
        ?? throw new ArgumentNullException("SENDGRID_NEWSLETTER_LIST_ID");
}