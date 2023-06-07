using System.Text.Json.Serialization;

namespace Api.Models;

public class MessageForm : ApiModelBase {
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
}