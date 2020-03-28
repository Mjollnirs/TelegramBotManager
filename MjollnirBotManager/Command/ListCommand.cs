using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using HtmlBuilders;
using MjollnirBotManager.Common.Command;
using MjollnirBotManager.Common.Store;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.Command
{
    public class ListCommand : CommandBase
    {
        private readonly IChatManager _chatManager;

        public ListCommand(IKernel kernel, IChatManager chatManager)
            : base(kernel)
        {
            _chatManager = chatManager;
        }

        protected async override Task Process(CancellationToken token)
        {
            if (!IsAdmin)
                return;

            string message = HtmlTags.B.Append("Chat List").ToHtmlString();

            foreach (var item in await _chatManager.GetAllChatsAsync())
            {
                var chat = await BotManager.Telegram.GetChatAsync(item, token);

                string _link = $"tg://user?id={chat.Id}";
                if (chat.Type != ChatType.Private)
                    _link = chat.InviteLink;

                message += $@"
* {HtmlTags.A.Href(_link).Append("Link.").ToHtmlString()}
    - Type: {chat.Type.ToString()}
";
                if (chat.Username != null && chat.Username != string.Empty)
                    message += $"    - UserName: @{chat.Username}\n";

                if (chat.Title != null && chat.Title != string.Empty)
                    message += $"    - Title: {chat.Title}\n";

                if (chat.Description != null && chat.Description != string.Empty)
                    message += $"    - Description: {chat.Description}\n";

            }

            await BotManager.Telegram.SendTextMessageAsync(Chat, message, parseMode: ParseMode.Html);
        }
    }
}
