using Api.Models;
using SendGrid.Helpers.Mail;

namespace Api.Interfaces;

public interface ISendGridMessageHandler
{
    SendGridMessage MakeSendGridMessage(MessageForm contact);
}
