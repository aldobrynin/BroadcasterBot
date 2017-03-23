using System;
using System.Threading.Tasks;
using BroadcasterBot.Data;
using BroadcasterBot.Dialogs.Factory;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog
    {
        private readonly IDialogFactory _dialogFactory;
        private readonly IUsersConversationsRepository _repository;
        public RootDialog(IDialogFactory dialogFactory, IUsersConversationsRepository repository)
        {
            _dialogFactory = dialogFactory;
            _repository = repository;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome!");
            var conversation = context.Activity.ToConversationReference();

            await _repository.AddUser(new SavedConversationDto(conversation.ServiceUrl, conversation.ChannelId,
                conversation.Conversation.Id, conversation.Bot.Id, conversation.User.Id));
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
                context.Wait(MessageReceivedAsync);
            }
        }

        private Task OnAfterBroadcastingResume(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
    }
}