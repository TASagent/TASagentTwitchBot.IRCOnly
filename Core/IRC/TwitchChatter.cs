using System;
using System.Threading.Tasks;

namespace TASagentTwitchBot.Core.IRC
{
    public record TwitchChatter
    {
        public string UserName { get; init; }
        public string Color { get; init; }
        public DateTime? CreatedAt { get; init; }
        public string Badges { get; init; }
        public string Message { get; init; }
        public string MessageId { get; init; }
        public bool Whisper { get; init; }
        public int Bits { get; init; }

        public static Task<TwitchChatter> FromIRCMessage(IRCMessage message)
        {
            if (message.ircCommand != IRCCommand.PrivMsg && message.ircCommand != IRCCommand.Whisper)
            {
                return null;
            }

            string displayName = message.tags["display-name"];

            if (string.IsNullOrWhiteSpace(displayName) || displayName == "1")
            {
                displayName = message.user;
            }

            string color = message.tags.ContainsKey("color") ? message.tags["color"] : null;
            int bits = message.tags.ContainsKey("bits") ? int.Parse(message.tags["bits"]) : 0;

            if (message.ircCommand == IRCCommand.Whisper)
            {
                return Task.FromResult(new TwitchChatter()
                {
                    UserName = displayName,
                    Color = color,
                    CreatedAt = DateTime.Now,
                    Badges = message.tags["badges"],
                    Message = message.message,
                    MessageId = null,
                    Whisper = true,
                    Bits = bits
                });
            }
            else
            {
                return Task.FromResult(new TwitchChatter()
                {
                    UserName = displayName,
                    Color = color,
                    CreatedAt = DateTime.Now,
                    Badges = message.tags["badges"],
                    Message = message.message,
                    MessageId = message.tags["id"],
                    Whisper = false,
                    Bits = bits
                });
            }
        }

        public string ToLogString() => Whisper ? $"[{CreatedAt:G}] {UserName} WHISPER: {Message}" : $"[{CreatedAt:G}] {UserName}: {Message}";
    }
}
