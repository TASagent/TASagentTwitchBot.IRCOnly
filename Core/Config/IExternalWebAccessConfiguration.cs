using System.Threading.Tasks;

namespace TASagentTwitchBot.Core.Config
{
    public interface IExternalWebAccessConfiguration
    {
        Task<string> GetExternalAddress();
        Task<string> GetExternalWebSubAddress();
        string GetLocalAddress();
    }
}
