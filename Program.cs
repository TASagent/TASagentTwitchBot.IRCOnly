using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TASagentTwitchBot.IRCOnly
{
    public class Program
    {
        public static async Task VerifySetup(
            Core.Config.IBotConfigContainer botConfigContainer,
            Core.API.Twitch.HelixHelper helixHelper,
            Core.API.Twitch.IBotTokenValidator botTokenValidator)
        {
            botConfigContainer.SerializeData();
            Core.Config.BotConfiguration botConfig = botConfigContainer.BotConfig;

            //Check Username
            if (string.IsNullOrEmpty(botConfig.Broadcaster))
            {
                Console.Write("Enter the Broadcaster's Username:\n    > ");
                string inputUserName = Console.ReadLine();

                inputUserName = inputUserName.Trim();

                if (string.IsNullOrEmpty(inputUserName))
                {
                    Console.WriteLine("\n\nError: Empty Broadcaster Username received. Aborting.");
                    Environment.Exit(1);
                }

                botConfig.Broadcaster = inputUserName;

                botConfigContainer.SerializeData();
            }

            //Check Bot
            if (string.IsNullOrEmpty(botConfig.BotName))
            {
                Console.Write("Enter the Bot's Username:\n    > ");
                string inputUserName = Console.ReadLine();

                inputUserName = inputUserName.Trim();

                if (string.IsNullOrEmpty(inputUserName))
                {
                    Console.WriteLine("\n\nError: Empty Bot Username received. Aborting.");
                    Environment.Exit(1);
                }

                botConfig.BotName = inputUserName;

                botConfigContainer.SerializeData();
            }

            //App Secrets
            if (string.IsNullOrEmpty(botConfig.TwitchClientId))
            {
                Console.Write("Enter the Twitch Client ID received from https://dev.twitch.tv/console/apps \n    > ");
                string clientID = Console.ReadLine();

                clientID = clientID.Trim();

                if (string.IsNullOrEmpty(clientID))
                {
                    Console.WriteLine("\n\nError: Empty Twitch ClientID received. Aborting.");
                    Environment.Exit(1);
                }

                botConfig.TwitchClientId = clientID;

                botConfigContainer.SerializeData();
            }

            if (string.IsNullOrEmpty(botConfig.TwitchClientSecret))
            {
                Console.Write("Enter the Twitch Client Secret received from https://dev.twitch.tv/console/apps \n    > ");
                string clientSecret = Console.ReadLine();

                clientSecret = clientSecret.Trim();

                if (string.IsNullOrEmpty(clientSecret))
                {
                    Console.WriteLine("\n\nError: Empty Twitch Client Secret received. Aborting.");
                    Environment.Exit(1);
                }

                botConfig.TwitchClientSecret = clientSecret;

                botConfigContainer.SerializeData();
            }

            //Try to connect and validate tokens
            if (await botTokenValidator.TryToConnect())
            {
                Console.WriteLine("Bot Token successfully validates");
            }
            else
            {
                Console.WriteLine("Unable to connect to Twitch");
                Console.WriteLine("Please check bot credentials and try again.");
                Console.WriteLine("Exiting bot configurator now...");
                Environment.Exit(1);
            }

            //Set broadcasterID if it's not set
            if (string.IsNullOrEmpty(botConfig.BroadcasterId))
            {
                //Fetch the Broadcaster ID
                botConfig.BroadcasterId = (await helixHelper.GetUsers(null, new List<string>() { botConfig.Broadcaster })).Data[0].ID;

                botConfigContainer.SerializeData();
            }
        }


        public static void Main(string[] args)
        {
            //Initialize DataManagement
            BGC.IO.DataManagement.Initialize("TASagentBotIRCOnly");

            IWebHost host = WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://0.0.0.0:5000")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.StartAsync().Wait();

            Core.ICommunication communication = host.Services.GetService(typeof(Core.ICommunication)) as Core.ICommunication;

            BGC.Debug.LogCallback += communication.SendDebugMessage;
            BGC.Debug.LogWarningCallback += communication.SendWarningMessage;
            BGC.Debug.LogErrorCallback += communication.SendErrorMessage;

            VerifySetup(
                botConfigContainer: host.Services.GetService(typeof(Core.Config.IBotConfigContainer)) as Core.Config.IBotConfigContainer,
                helixHelper: host.Services.GetService(typeof(Core.API.Twitch.HelixHelper)) as Core.API.Twitch.HelixHelper,
                botTokenValidator: host.Services.GetService(typeof(Core.API.Twitch.IBotTokenValidator)) as Core.API.Twitch.IBotTokenValidator).Wait();

            IRCOnlyApplication application = host.Services.GetService(typeof(IRCOnlyApplication)) as IRCOnlyApplication;
            application.RunAsync().Wait();

            host.StopAsync().Wait();

            host.Dispose();
        }
    }
}
