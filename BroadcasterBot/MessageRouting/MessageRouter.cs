using System;
using System.Diagnostics;
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

        public async Task SendToAllUsers(IMessageActivity activity)
        {
            var users = _repository.GetAllUsers();
            var tasks = users.Select(user => CreateTask(activity, user));
            await Task.WhenAll(tasks);
        }

        private async Task CreateTask(IMessageActivity activity, ConversationReference user)
        {
            try
            {
                using (var client = new ConnectorClient(new Uri(user.ServiceUrl)))
                {
                    var replyActivity = CreateReplyActivity(activity, user);
                    await client.Conversations.SendToConversationAsync(replyActivity);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
        }

        private static Activity CreateReplyActivity(IMessageActivity activity, ConversationReference user)
        {
            return new Activity
            {
                From = user.Bot,
                Recipient = user.User,
                ChannelId = user.ChannelId,
                ServiceUrl = user.ServiceUrl,
                Conversation = user.Conversation,
                Type = ActivityTypes.Message,
                Text = activity.Text
            };
        }

        public async Task SendToAllUsers<T>(IDialog<T> dialog)
        {
            var users = _repository.GetAllUsers();
            foreach (var conversationReferences in users)
            {
                var message = conversationReferences.GetPostToBotMessage();

                using (var scope = DialogModule.BeginLifetimeScope(_scope, message))
                {
                    var botData = scope.Resolve<IBotData>();
                    var token = new CancellationToken();
                    await botData.LoadAsync(token);

                    //This is our dialog stack
                    var stack = scope.Resolve<IDialogTask>();

                    //interrupt the stack. This means that we're stopping whatever conversation that is currently happening with the user
                    //Then adding this stack to run and once it's finished, we will be back to the original conversation
                    stack.Call(dialog.Void<T, IMessageActivity>(), null);
                    await stack.PollAsync(token);

                    //flush dialog stack
                    await botData.FlushAsync(token);
                }
            }
        }
    }
}