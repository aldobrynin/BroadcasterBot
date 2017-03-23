using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class BroadcasterDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Let's broadcast!");
            context.Wait(OnMessageReceived);
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            await context.PostAsync("Your message will be broadcasted");
            context.Wait(OnMessageReceived);
        }
    }
}