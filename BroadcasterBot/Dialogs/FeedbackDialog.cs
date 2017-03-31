using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class FeedbackDialog : IDialog
    {
        private readonly string _message;

        public FeedbackDialog(string message)
        {
            _message = message;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_message);
            PromptDialog.Confirm(context, Resume, "Are u ok?");
        }

        private async Task Resume(IDialogContext context, IAwaitable<bool> result)
        {
            var value = await result;
            context.Done(value);
        }
    }
}