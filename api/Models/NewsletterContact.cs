using System.Text.Json.Serialization;

namespace Api.Models;

public class NewsletterContact : ApiModelBase
{
    public string Email { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    public NewsletterContact()
    {
        Email = string.Empty;
    } 
}