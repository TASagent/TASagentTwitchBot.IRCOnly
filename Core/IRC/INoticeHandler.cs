namespace TASagentTwitchBot.Core.IRC
{
    public interface INoticeHandler
    {
        void HandleIRCNotice(IRCMessage message);
    }
}
