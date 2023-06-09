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
        _configuration["AzureWebJobsSendGridApiKey"]
        ?? throw new ArgumentNullException("AzureWebJobsSendGridApiKey");

    public string NewsletterListId =>
        _configuration["SENDGRID_NEWSLETTER_LIST_ID"]
        ?? throw new ArgumentNullException("SENDGRID_NEWSLETTER_LIST_ID");
    
    public string TemplateId =>
        _configuration["SENDGRID_TEMPLATE_ID"]
        ?? throw new ArgumentNullException("SENDGRID_TEMPLATE_ID");

    public string EmailAddress =>
        _configuration["SENDGRID_EMAIL_ADDRESS"]
        ?? throw new ArgumentNullException("SENDGRID_EMAIL_ADDRESS");

    public string SenderName =>
        _configuration["SENDGRID_SENDER_NAME"]
        ?? throw new ArgumentNullException("SENDGRID_SENDER_NAME");

    public string SubjectLine =>
        _configuration["SENDGRID_SUBJECT_LINE"]
        ?? throw new ArgumentNullException("SENDGRID_SUBJECT_LINE");

    public string WebsiteAdminEmail =>
        _configuration["WEBSITE_ADMIN_EMAIL"]
        ?? throw new ArgumentNullException("WEBSITE_ADMIN_EMAIL");

    public int SuppressionGroupId =>
        int.Parse(_configuration["SENDGRID_SUPPRESSION_GROUP_ID"]
        ?? throw new ArgumentNullException("SENDGRID_SUPPRESSION_GROUP_ID"));
}