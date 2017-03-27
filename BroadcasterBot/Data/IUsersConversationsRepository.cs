using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Data
{
    public interface IUsersConversationsRepository
    {
        IEnumerable<ConversationReference> GetAllUsers();
        void AddUser(ConversationReference conversation);
        void SetBroadcaster(string userId, string channelId, bool isBroadcaster);
    }
}