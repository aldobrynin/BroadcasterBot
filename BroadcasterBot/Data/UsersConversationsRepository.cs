using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BroadcasterBot.Data
{
    public class UsersConversationsRepository : IUsersConversationsRepository
    {
        public Task<IEnumerable<SavedConversationDto>> GetAllUsers()
        {
            using (var db = new UsersConversationsContext())
            {
                return Task.FromResult(db.UsersConversations.AsEnumerable());
            }
        }

        public async Task AddUser(SavedConversationDto conversation)
        {
            using (var db = new UsersConversationsContext())
            {
                if (db.UsersConversations.Any(x => x.UserId == conversation.UserId) == false)
                {
                    db.UsersConversations.Add(conversation);
                    await db.SaveChangesAsync();
                }
            }
        }
    }

    public class UsersConversationsContext : DbContext
    {
        public UsersConversationsContext() : base("Main")
        {
        }

        public DbSet<SavedConversationDto> UsersConversations { get; set; }
    }
}