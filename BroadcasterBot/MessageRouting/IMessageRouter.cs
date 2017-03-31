using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.MessageRouting
{
    public interface IMessageRouter
    {
        Task SendToAllUsers(IMessageActivity activity);
        Task SendToAllUsers<T>(IDialog<T> dialog);
    }
}