using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BroadcasterBot.MessageRouting;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs.Broadcaster
{
    [Serializable]
    public class SendMessageDialog : IDialog
    {
        private readonly IMessageRouter _router;

        public SendMessageDialog(IMessageRouter router)
        {
            _router = router;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Enter your message:");
            context.Wait(OnMessageReceivedAsync);
        }

        private async Task OnMessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            try
            {
                await _router.SendToAllUsers(activity);
                await context.PostAsync("Your message has been broadcasted");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                await context.PostAsync("An error has occured while sending message");
            }

            context.Done(true);
        }
    }
}