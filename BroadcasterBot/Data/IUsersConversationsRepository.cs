using System.Collections.Generic;

namespace BroadcasterBot.Data
{
    public interface IUsersConversationsRepository
    {
        IEnumerable<SavedConversationDto> GetAllUsers();
        SavedConversationDto FindByUserIdAndChannelId(string userId, string channelId);
        void AddUser(SavedConversationDto conversation);
    }
}