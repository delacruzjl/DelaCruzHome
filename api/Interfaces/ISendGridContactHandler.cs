using System.Threading.Tasks;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Interfaces;

public interface ISendGridContactHandler
{
    Task<int> AddContactToSendGridGroup(NewsletterContact contact, ILogger logger);
    Task<int> AddContactToSendGridList(NewsletterContact contact, ILogger logger);
}
