using System;
using System.Threading.Tasks;
using BroadcasterBot.MessageRouting;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class BroadcasterDialog : IDialog
    {
        private readonly IMessageRouter _router;

        public BroadcasterDialog(IMessageRouter router)
        {
            _router = router;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Let's broadcast!");
            context.Wait(OnMessageReceived);
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            var isBroadcastSuccessfull = await _router.SendToAllUsers(activity);
            if (isBroadcastSuccessfull)
                await context.PostAsync("Your message has been broadcasted");
            else
                await context.PostAsync("An error has occured while sending message");
            context.Wait(OnMessageReceived);
        }
    }
}