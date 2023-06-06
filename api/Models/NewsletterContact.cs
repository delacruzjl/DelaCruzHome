using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class NewsletterContact
{
    [EmailAddress]
    public string Email { get; set; }

    public NewsletterContact()
    {
        Email = string.Empty;
    } 
}