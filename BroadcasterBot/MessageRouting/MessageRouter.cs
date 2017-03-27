using System;
using System.Linq;
using System.Threading;
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

        public async Task SendToAllUsers(IMessageActivity activity, CancellationToken token = default(CancellationToken))
        {
            var users = _repository.GetAllUsers();
            foreach (var serviceUsers in users.GroupBy(x => x.ServiceUrl))
            {
                using (var client = new ConnectorClient(new Uri(serviceUsers.Key)))
                {
                    foreach (var user in serviceUsers)
                    {
                        var replyActivity = new Activity
                        {
                            From = user.Bot,
                            Recipient = user.User,
                            ChannelId = user.ChannelId,
                            ServiceUrl = user.ServiceUrl,
                            Conversation = user.Conversation,
                            Type = ActivityTypes.Message,
                            Text = activity.Text,
                            Attachments = activity.Attachments,
                            AttachmentLayout = activity.AttachmentLayout,
                            TextFormat = activity.TextFormat,
                            Locale = activity.Locale,
                            Summary = activity.Summary,
                            Entities = activity.Entities
                        };

                        await client.Conversations.SendToConversationAsync(replyActivity, token);
                    }
                }
            }
        }
    }
}