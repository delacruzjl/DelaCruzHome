using System;
using Microsoft.Extensions.Configuration;

namespace Api.Models;

public class SendGridConfiguration  {
    private readonly IConfiguration _configuration;

    public SendGridConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string ApiKey =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_API_KEY"])
            ? _configuration["SENDGRID_API_KEY"]
            : throw new ArgumentNullException("SENDGRID_API_KEY");

    public string NewsletterListId =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_NEWSLETTER_LIST_ID"])
        ? _configuration["SENDGRID_NEWSLETTER_LIST_ID"]
        : throw new ArgumentNullException("SENDGRID_NEWSLETTER_LIST_ID");
    
    public string TemplateId =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_TEMPLATE_ID"])
        ? _configuration["SENDGRID_TEMPLATE_ID"]
        : throw new ArgumentNullException("SENDGRID_TEMPLATE_ID");

    public string EmailAddress =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_EMAIL_ADDRESS"])
        ? _configuration["SENDGRID_EMAIL_ADDRESS"]
        : throw new ArgumentNullException("SENDGRID_EMAIL_ADDRESS");

    public string SenderName =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_SENDER_NAME"])
        ? _configuration["SENDGRID_SENDER_NAME"]
        : throw new ArgumentNullException("SENDGRID_SENDER_NAME");

    public string SubjectLine =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_SUBJECT_LINE"])
        ? _configuration["SENDGRID_SUBJECT_LINE"]
        : throw new ArgumentNullException("SENDGRID_SUBJECT_LINE");

    public string WebsiteAdminEmail =>
        string.IsNullOrWhiteSpace(_configuration["WEBSITE_ADMIN_EMAIL"])
        ? _configuration["WEBSITE_ADMIN_EMAIL"]
        : throw new ArgumentNullException("WEBSITE_ADMIN_EMAIL");

    public int SuppressionGroupId =>
        string.IsNullOrWhiteSpace(_configuration["SENDGRID_SUPPRESSION_GROUP_ID"])
        ? int.Parse(_configuration["SENDGRID_SUPPRESSION_GROUP_ID"])
        : throw new ArgumentNullException("SENDGRID_SUPPRESSION_GROUP_ID");
}