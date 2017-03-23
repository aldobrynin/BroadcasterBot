using System;
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

        public async Task<bool> SendToAllUsers(IMessageActivity activity)
        {
            var users = await _repository.GetAllUsers();
            foreach (var grouping in users.GroupBy(x => x.ServiceUrl))
                using (var client = new ConnectorClient(new Uri(grouping.Key)))
                {
                    foreach (var reference in grouping)
                    {
                        var replyActivity = CreateActivity(activity, reference);
                        try
                        {
                            await client.Conversations.SendToConversationAsync(replyActivity);
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
            return true;
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
    }
}