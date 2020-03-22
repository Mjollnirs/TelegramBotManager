namespace MjollnirBotManager.Common.Configuration
{
    public interface IBotConfiguration
    {
        string ApiToken { get; set; }
        long RootAdminChat { get; set; }
        long DefaultChat { get; set; }
        ProxyConfiguration Proxy { get; set; }
    }
}