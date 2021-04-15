using System;

namespace TASagentTwitchBot.IRC
{
    public class PointlessNoticeHandler : Core.IRC.INoticeHandler
    {
        public PointlessNoticeHandler() { }

        public void HandleIRCNotice(Core.IRC.IRCMessage message)
        {
            string noticeType = message.tags["msg-id"];

            switch (noticeType)
            {
                case "sub":
                case "resub":
                    //Do Nothing
                    break;

                case "subgift":
                    //Do Nothing
                    break;

                case "anonsubgift":
                    //Do Nothing
                    break;

                case "raid":
                    //Do Nothing
                    break;

                case "submysterygift":
                case "giftpaidupgrade":
                case "rewardgift":
                case "anongiftpaidupgrade":
                case "unraid":
                case "ritual":
                case "bitsbadgetier":
                case "host_on":
                case "host_off":
                case "host_target_went_offline":
                    //Do Nothing
                    break;

                default:
                    //Do Nothing
                    break;
            }
        }
    }
}
