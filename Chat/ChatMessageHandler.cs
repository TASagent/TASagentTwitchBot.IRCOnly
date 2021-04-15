
namespace TASagentTwitchBot.Chat
{
    public class ChatMessageHandler : Core.Chat.IChatMessageHandler
    {
        private readonly Core.ICommunication communication;

        public ChatMessageHandler(
            Core.ICommunication communication)
        {
            this.communication = communication;
        }

        public virtual async void HandleChatMessage(Core.IRC.IRCMessage message)
        {
            if (message.ircCommand != Core.IRC.IRCCommand.PrivMsg && message.ircCommand != Core.IRC.IRCCommand.Whisper)
            {
                communication.SendDebugMessage($"Error: Passing forward non-chat message:\n    {message}");
                return;
            }

            Core.IRC.TwitchChatter chatter = await Core.IRC.TwitchChatter.FromIRCMessage(message);

            if (chatter == null)
            {
                return;
            }

            communication.DispatchChatMessage(chatter);
        }
    }

}
