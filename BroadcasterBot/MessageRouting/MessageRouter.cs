using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using BroadcasterBot.Data;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.MessageRouting
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IUsersConversationsRepository _repository;
        private readonly ILifetimeScope _scope;
        public MessageRouter(IUsersConversationsRepository repository, ILifetimeScope scope)
        {
            _repository = repository;
            _scope = scope;
        }

        public async Task SendToAllUsers(IMessageActivity activity, CancellationToken token = default(CancellationToken))
        {
            var users = _repository.GetAllUsers();
            foreach (var grouping in users.GroupBy(x => x.ServiceUrl))
            {
                //using (var client = new ConnectorClient(new Uri(grouping.Key)))
                //{
                    foreach (var reference in grouping)
                    {
                        var replyActivity = CreateActivity(activity, reference);
                        //await client.Conversations.SendToConversationAsync(replyActivity, token);
                        var message = GetPostToBotMessage(reference);

                        using (var scope = DialogModule.BeginLifetimeScope(_scope, message))
                        {
                            var botData = scope.Resolve<IBotData>();
                            await botData.LoadAsync(token);

                            //This is our dialog stack
                            var stack = scope.Resolve<IDialogTask>();

                            //interrupt the stack. This means that we're stopping whatever conversation that is currently happening with the user
                            //Then adding this stack to run and once it's finished, we will be back to the original conversation
                            var dialog = new NotificationDialog(replyActivity);
                            stack.Call(dialog.Void<object, IMessageActivity>(), null);
                            await stack.PollAsync(token);

                            //flush dialog stack
                            await botData.FlushAsync(token);
                        }
                    }
                //}
            }
        }

        private static Activity CreateActivity(IMessageActivity activity, SavedConversationDto reference)
        {
            return new Activity
            {
                Type = ActivityTypes.Message,
                Text = activity.Text,
                From = new ChannelAccount(reference.BotId),
                Recipient = new ChannelAccount(reference.UserId),
                ChannelId = reference.ChannelId,
                ServiceUrl = reference.ServiceUrl,
                Conversation = new ConversationAccount(id: reference.ConversationId),
                Attachments = activity.Attachments,
                AttachmentLayout = activity.AttachmentLayout,
                Locale = activity.Locale,
                TextFormat = activity.TextFormat,
                Summary = activity.Summary,
                Entities = activity.Entities
            };
        }

        private static IMessageActivity GetPostToBotMessage(SavedConversationDto reference)
        {
            return new Activity
            {
                Type = ActivityTypes.Message,
                Id = Guid.NewGuid().ToString(),
                Recipient = new ChannelAccount
                {
                    Id = reference.BotId
                },
                ChannelId = reference.ChannelId,
                ServiceUrl = reference.ServiceUrl,
                Conversation = new ConversationAccount
                {
                    Id = reference.ConversationId
                },
                From = new ChannelAccount
                {
                    Id = reference.UserId,
                    Name = reference.UserId
                }
            };
        }
    }

    [Serializable]
    public class NotificationDialog : IDialog
    {
        private readonly IMessageActivity _message;
        public NotificationDialog(IMessageActivity message)
        {
            _message = message;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_message);
            context.Done(true);
        }
    }
}