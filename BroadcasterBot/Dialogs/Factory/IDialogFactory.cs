using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs.Factory
{
    public interface IDialogFactory
    {
        T Create<T>(IMessageActivity messageActivity);
    }
}