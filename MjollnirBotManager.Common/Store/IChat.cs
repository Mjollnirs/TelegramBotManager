using LiteDB;

namespace MjollnirBotManager.Common.Store
{
    public interface IChat
    {
        ObjectId Id { get; set; }
        long Identifier { get; set; }
        bool IsAdmin { get; set; }
    }
}