using System;
using System.Threading.Tasks;
using BroadcasterBot.Dialogs.Factory;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog
    {
        private readonly IDialogFactory _dialogFactory;

        public RootDialog(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome!");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            if (activity.Text == "i wanna broadcast")
            {
                var dialog = _dialogFactory.Create<BroadcasterDialog>(activity);
                context.Call(dialog, OnAfterBroadcastingResume);
            }
            else
            {
                await context.PostAsync("shhhhh... just listen");
            }
        }

        private Task OnAfterBroadcastingResume(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
    }
}