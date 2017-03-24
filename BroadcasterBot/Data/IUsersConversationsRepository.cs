using System.Collections.Generic;
using System.Threading.Tasks;

namespace BroadcasterBot.Data
{
    public interface IUsersConversationsRepository
    {
        Task<IEnumerable<SavedConversationDto>> GetAllUsers();
        Task AddUser(SavedConversationDto conversation);
    }
}