using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BroadcasterBot.Data;
using BroadcasterBot.MessageRouting;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class BroadcasterDialog : IDialog
    {
        private readonly IUsersConversationsRepository _repository;
        private readonly IMessageRouter _router;

        public BroadcasterDialog(IMessageRouter router, IUsersConversationsRepository repository)
        {
            _router = router;
            _repository = repository;
        }

        public async Task StartAsync(IDialogContext context)
        {
            _repository.SetBroadcaster(context.Activity.From.Id, context.Activity.ChannelId, true);

            await context.PostAsync("Let's broadcast!");
            context.Wait(OnMessageReceived);
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            if (activity.Text == "exit")
            {
                _repository.SetBroadcaster(context.Activity.From.Id, context.Activity.ChannelId, false);
                context.Done(true);
                return;
            }

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
            context.Wait(OnMessageReceived);
        }
    }
}