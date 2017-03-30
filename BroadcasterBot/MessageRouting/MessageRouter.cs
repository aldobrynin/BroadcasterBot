using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BroadcasterBot.Data;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.MessageRouting
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IUsersConversationsRepository _repository;

        public MessageRouter(IUsersConversationsRepository repository)
        {
            _repository = repository;
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
    }
}