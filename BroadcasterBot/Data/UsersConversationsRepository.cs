using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

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

        public IEnumerable<SavedConversationDto> GetAllUsers()
        {
            return _context.UsersConversations.Where(x => x.IsBroadcaster == false);
        }

        public void AddUser(SavedConversationDto conversation)
        {
            _context.UsersConversations.AddOrUpdate(conversation);
            _context.SaveChanges();
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

        public SavedConversationDto FindByUserIdAndChannelId(string userId, string channelId)
        {
            return _context.UsersConversations.SingleOrDefault(x => x.UserId == userId && x.ChannelId == channelId);
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