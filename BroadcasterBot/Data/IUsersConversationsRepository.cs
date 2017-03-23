using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Data
{
    public interface IUsersConversationsRepository
    {
        Task<IEnumerable<SavedConversationDto>> GetAllUsers();
        Task<bool> AddUser(SavedConversationDto conversation);
    }
}