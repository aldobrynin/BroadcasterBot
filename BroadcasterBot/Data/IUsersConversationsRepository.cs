using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Data
{
    public interface IUsersConversationsRepository
    {
        Task<IEnumerable<ConversationReference>> GetAllUsers();
        Task<bool> AddUser(ConversationReference conversation);
    }
}