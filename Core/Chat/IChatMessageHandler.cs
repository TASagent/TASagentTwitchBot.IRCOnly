namespace TASagentTwitchBot.Core.Chat
{
    public interface IChatMessageHandler
    {
        void HandleChatMessage(IRC.IRCMessage message);
    }

}
