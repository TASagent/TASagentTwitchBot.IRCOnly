using System;
using System.Threading.Tasks;

namespace TASagentTwitchBot.IRCOnly
{
    public class IRCOnlyApplication
    {
        private readonly Core.ICommunication communication;
        private readonly Core.ErrorHandler errorHandler;
        private readonly Core.ApplicationManagement applicationManagement;

        private readonly Core.IRC.IrcClient ircClient;
        private readonly Core.API.Twitch.IBotTokenValidator botTokenValidator;
        //private readonly Core.API.Twitch.IBroadcasterTokenValidator broadcasterTokenValidator;

        public IRCOnlyApplication(
            Core.ICommunication communication,
            Core.ErrorHandler errorHandler,
            Core.ApplicationManagement applicationManagement,
            Core.IRC.IrcClient ircClient,
            //Core.API.Twitch.IBroadcasterTokenValidator broadcasterTokenValidator,
            Core.API.Twitch.IBotTokenValidator botTokenValidator)
        {
            this.communication = communication;
            this.errorHandler = errorHandler;
            this.applicationManagement = applicationManagement;

            this.ircClient = ircClient;
            this.botTokenValidator = botTokenValidator;
            //this.broadcasterTokenValidator = broadcasterTokenValidator;

            BGC.Debug.ExceptionCallback += errorHandler.LogExternalException;
        }

        public async Task RunAsync()
        {
            try
            {
                communication.SendDebugMessage("*** Starting Up ***");
                communication.SendDebugMessage("Connecting to Twitch");

                if (!await botTokenValidator.TryToConnect())
                {
                    communication.SendErrorMessage("------------> URGENT <------------");
                    communication.SendErrorMessage("Please check bot credential process and try again.");
                    communication.SendErrorMessage("Unable to connect to Twitch");
                    communication.SendErrorMessage("Exiting bot application now...");
                    await Task.Delay(7500);
                    Environment.Exit(1);
                }

                //if (!await broadcasterTokenValidator.TryToConnect())
                //{
                //    communication.SendErrorMessage("------------> URGENT <------------");
                //    communication.SendErrorMessage("Please check broadcaster credential process and try again.");
                //    communication.SendErrorMessage("Unable to connect to Twitch");
                //    communication.SendErrorMessage("Exiting bot application now...");
                //    await Task.Delay(7500);
                //    Environment.Exit(1);
                //}


                communication.SendDebugMessage("Connecting to IRC");

                await ircClient.Start();

                //Kick off Validator
                botTokenValidator.RunValidator();
                //broadcasterTokenValidator.RunValidator();

                communication.SendPublicChatMessage("I have connected.");
            }
            catch (Exception ex)
            {
                errorHandler.LogFatalException(ex);
            }

            try
            {
                await applicationManagement.WaitForEndAsync();
            }
            catch (Exception ex)
            {
                errorHandler.LogSystemException(ex);
            }
        }
    }
}
