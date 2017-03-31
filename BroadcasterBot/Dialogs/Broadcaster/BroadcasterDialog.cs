using System;
using System.Linq;
using System.Threading.Tasks;
using BroadcasterBot.Data;
using BroadcasterBot.Dialogs.Factory;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Dialogs.Broadcaster
{
    [Serializable]
    public class BroadcasterDialog : IDialog
    {
        private readonly IDialogFactory _factory;
        private readonly IUsersConversationsRepository _repository;

        public BroadcasterDialog(IUsersConversationsRepository repository, IDialogFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public async Task StartAsync(IDialogContext context)
        {
            _repository.SetBroadcaster(context.Activity.From.Id, context.Activity.ChannelId, true);
            await ShowPrompt(context);
        }

        private async Task ShowPrompt(IDialogContext context)
        {
            var buttons =
                BroadcasterCommand.SupportedBroadcasterCommands.Select(
                        x => new CardAction(ActionTypes.PostBack, x.Description, value: x.Value))
                    .ToList();
            var card = new HeroCard("Let's broadcast!", buttons: buttons);
            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());
            await context.PostAsync(message);
            context.Wait(OnMessageReceived);
        }

        private async Task OnMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            switch (activity.Text)
            {
                case BroadcasterCommand.Exit:
                    _repository.SetBroadcaster(context.Activity.From.Id, context.Activity.ChannelId, false);
                    await context.PostAsync("bye :(");
                    context.Done(true);
                    return;
                case BroadcasterCommand.SendFeedback:
                    context.Call(_factory.Create<SendFeedbackDialog>(activity), ResumeAsync);
                    break;
                case BroadcasterCommand.SendMessage:
                    context.Call(_factory.Create<SendMessageDialog>(activity), ResumeAsync);
                    break;
                default:
                    context.Wait(OnMessageReceived);
                    break;
            }
        }

        private async Task ResumeAsync(IDialogContext context, IAwaitable<object> result)
        {
            await ShowPrompt(context);
        }
    }
}