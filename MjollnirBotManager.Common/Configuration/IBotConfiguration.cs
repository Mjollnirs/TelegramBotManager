namespace MjollnirBotManager.Common.Configuration
{
    public interface IBotConfiguration
    {
        string ApiToken { get; set; }
        string RootAdmin { get; set; }
        long DefaultChat { get; set; }
        ProxyConfiguration Proxy { get; set; }
    }
}