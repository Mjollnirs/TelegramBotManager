using System;
using System.Collections.Generic;

namespace MjollnirBotManager.Common.Security
{
    internal sealed class Session : Dictionary<string, object>, ISession
    {
    }
}
