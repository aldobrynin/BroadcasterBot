using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace BroadcasterBot.Data
{
    public class UsersConversationsRepository : IUsersConversationsRepository, IDisposable
    {
        private readonly UsersConversationsContext _context;
        private bool _disposed;

        public UsersConversationsRepository()
        {
            _context = new UsersConversationsContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IEnumerable<SavedConversationDto>> GetAllUsers()
        {
            return Task.FromResult(_context.UsersConversations.AsEnumerable());
        }

        public async Task AddUser(SavedConversationDto conversation)
        {
            _context.UsersConversations.AddOrUpdate(conversation);
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
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