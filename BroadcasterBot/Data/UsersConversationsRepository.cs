using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Data
{
    public class UsersConversationsRepository : IUsersConversationsRepository
    {
        private readonly HashSet<ConversationReference> _userReferences = new HashSet<ConversationReference>();

        public Task<IEnumerable<ConversationReference>> GetAllUsers()
        {
            return Task.FromResult(_userReferences.AsEnumerable());
        }

        public Task<bool> AddUser(ConversationReference conversation)
        {
            return Task.FromResult(_userReferences.Add(conversation));
        }
    }
}