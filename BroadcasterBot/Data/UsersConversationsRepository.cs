using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BroadcasterBot.Data
{
    public class UsersConversationsRepository : IUsersConversationsRepository
    {
        private readonly HashSet<SavedConversationDto> _userReferences = new HashSet<SavedConversationDto>();

        public Task<IEnumerable<SavedConversationDto>> GetAllUsers()
        {
            return Task.FromResult(_userReferences.AsEnumerable());
        }

        public Task<bool> AddUser(SavedConversationDto conversation)
        {
            return
                Task.FromResult(
                    _userReferences.Add(conversation));
        }
    }
}