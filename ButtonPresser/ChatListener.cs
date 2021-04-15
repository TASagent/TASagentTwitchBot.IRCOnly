using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TASagentTwitchBot.IRCOnly.ButtonPresser
{
    public class ChatListener
    {
        private readonly Core.ICommunication communication;
        private readonly IButtonPressDispatcher buttonPressDispatcher;

        public ChatListener(
            Core.ICommunication communication,
            IButtonPressDispatcher buttonPressDispatcher)
        {
            this.communication = communication;
            this.buttonPressDispatcher = buttonPressDispatcher;

            communication.ReceiveMessageHandlers += ReceiveMessageHandler;
        }

        private void ReceiveMessageHandler(Core.IRC.TwitchChatter chatter)
        {
            switch (chatter.Message.ToUpperInvariant())
            {
                case "F":
                    //Press F
                    buttonPressDispatcher.TriggerKeyPress(DirectXKeyStrokes.DIK_F, 50);
                    break;

                case "GG":
                    communication.SendPublicChatMessage($"Shut up, @{chatter.UserName}!");
                    break;

                default:
                    //Do Nothing
                    break;
            }
        }
    }
}
