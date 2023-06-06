using System;

namespace Api.Models;

public class SendGridConfiguration {
    public static string ApiKey =>
        Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? 
        throw new ArgumentException("SENDGRID_API_KEY is not set");

    public static string NewsletterListId =>
        Environment.GetEnvironmentVariable("SENDGRID_NEWSLETTER_LIST_ID") ?? 
        throw new ArgumentException("SENDGRID_NEWSLETTER_LIST_ID is not set");
}