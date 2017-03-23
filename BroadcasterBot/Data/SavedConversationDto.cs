using System;

namespace BroadcasterBot.Data
{
    public class SavedConversationDto : IEquatable<SavedConversationDto>
    {
        public SavedConversationDto(string serviceUrl, string channelId, string conversationId, string botId,
            string userId)
        {
            ServiceUrl = serviceUrl;
            ChannelId = channelId;
            ConversationId = conversationId;
            BotId = botId;
            UserId = userId;
        }

        public string ServiceUrl { get; }
        public string ChannelId { get; }
        public string ConversationId { get; }
        public string BotId { get; }
        public string UserId { get; }

        public bool Equals(SavedConversationDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ServiceUrl, other.ServiceUrl) && string.Equals(ChannelId, other.ChannelId) &&
                   string.Equals(ConversationId, other.ConversationId) && string.Equals(BotId, other.BotId) &&
                   string.Equals(UserId, other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SavedConversationDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ServiceUrl != null ? ServiceUrl.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (ChannelId != null ? ChannelId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ConversationId != null ? ConversationId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BotId != null ? BotId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserId != null ? UserId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}