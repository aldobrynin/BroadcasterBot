using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.MessageRouting
{
    public interface IMessageRouter
    {
        Task SendToAllUsers(IMessageActivity activity);
    }
}