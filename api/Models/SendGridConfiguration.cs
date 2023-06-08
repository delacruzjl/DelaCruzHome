using System;

namespace Api.Models;

public class SendGridConfiguration  {
    public static string ApiKey =>
        Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        ?? throw new ArgumentNullException("SENDGRID_API_KEY");

    public static string NewsletterListId =>
        Environment.GetEnvironmentVariable("SENDGRID_NEWSLETTER_LIST_ID")
        ?? throw new ArgumentNullException("SENDGRID_NEWSLETTER_LIST_ID");
    
    public static string TemplateId =>
        Environment.GetEnvironmentVariable("SENDGRID_TEMPLATE_ID")
        ?? throw new ArgumentNullException("SENDGRID_TEMPLATE_ID");

    public static string EmailAddress =>
        Environment.GetEnvironmentVariable("SENDGRID_EMAIL_ADDRESS")
        ?? throw new ArgumentNullException("SENDGRID_EMAIL_ADDRESS");

    public static string SenderName =>
        Environment.GetEnvironmentVariable("SENDGRID_SENDER_NAME")
        ?? throw new ArgumentNullException("SENDGRID_SENDER_NAME");

    public static string SubjectLine =>
        Environment.GetEnvironmentVariable("SENDGRID_SUBJECT_LINE")
        ?? throw new ArgumentNullException("SENDGRID_SUBJECT_LINE");

    public static string WebsiteAdminEmail =>
        Environment.GetEnvironmentVariable("WEBSITE_ADMIN_EMAIL")
        ?? throw new ArgumentNullException("WEBSITE_ADMIN_EMAIL");

    public static int SuppressionGroupId =>
        int.Parse(Environment.GetEnvironmentVariable("SENDGRID_SUPPRESSION_GROUP_ID")
        ?? throw new ArgumentNullException("SENDGRID_SUPPRESSION_GROUP_ID"));
}