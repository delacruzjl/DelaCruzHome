namespace Api.Models;

public class NewsletterContact : ApiModelBase
{
    public string Email { get; set; }

    public NewsletterContact()
    {
        Email = string.Empty;
    } 
}