using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.Bot.Connector;

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

        public IEnumerable<ConversationReference> GetAllUsers()
        {
            return _context.UsersConversations.Where(x => x.IsBroadcaster == false).ToArray().Select(Map);
        }

        public void AddUser(ConversationReference conversation)
        {
            _context.UsersConversations.AddOrUpdate(Map(conversation));
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

        private ConversationReference Map(SavedConversationDto dto)
        {
            return new ConversationReference
            {
                ChannelId = dto.ChannelId,
                User = new ChannelAccount(dto.UserId, dto.UserName),
                ServiceUrl = dto.ServiceUrl,
                Bot = new ChannelAccount(dto.BotId, dto.BotName),
                Conversation = new ConversationAccount(dto.IsGroupConversation, dto.ConversationId, dto.ConversationName)
            };
        }
        private SavedConversationDto Map(ConversationReference reference)
        {
            if (reference == null) return null;
            return new SavedConversationDto
            {
                ChannelId = reference.ChannelId,
                UserId = reference.User.Id,
                UserName = reference.User.Name,
                BotId = reference.Bot.Id,
                BotName = reference.Bot.Name,
                ConversationId = reference.Conversation.Id,
                ConversationName = reference.Conversation.Name,
                IsGroupConversation = reference.Conversation.IsGroup,
                ServiceUrl = reference.ServiceUrl,
            };
        }

        public void SetBroadcaster(string userId, string channelId, bool isBroadcaster)
        {
            var user = _context.UsersConversations.SingleOrDefault(x => x.UserId == userId && x.ChannelId == channelId);
            if (user == null) return;
            user.IsBroadcaster = isBroadcaster;
            _context.SaveChanges();
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