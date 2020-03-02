﻿using System;
namespace MjollnirBotManager.Common.Configuration
{
    public class BotConfiguration : IBotConfiguration
    {
        public string ApiToken { get; set; }

        public string RootAdmin { get; set; }

        public long DefaultChat { get; set; }

        public ProxyConfiguration Proxy { get; set; }
    }
}
