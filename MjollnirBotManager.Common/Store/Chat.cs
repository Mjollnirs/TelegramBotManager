using System;
using LiteDB;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Store
{
    internal sealed class Chat : IChat
    {
        public ObjectId Id { get; set; }

        public long Identifier { get; set; }

        public bool IsAdmin { get; set; }

        public Chat(ChatId chat, bool isAdmin = false)
            : this(chat.Identifier, isAdmin)
        {
        }

        public Chat(long chat, bool isAdmin = false)
        {
            Id = ObjectId.NewObjectId();
            Identifier = chat;
            IsAdmin = isAdmin;
        }
    }
}
