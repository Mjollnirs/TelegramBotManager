using System;
using System.Collections.Generic;

namespace MjollnirBotManager.Common.Security
{
    public interface ISession : ICollection<KeyValuePair<string, object>>
    {
    }
}
