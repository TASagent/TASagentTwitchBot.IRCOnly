# TASagentTwitchBot.IRCOnly

Bastardization of my modular C# [twitch bot development framework](https://github.com/TASagent/TASagentTwitchBotCore).  Specialized for [Darkanine](https://www.twitch.tb/Darkanine).

## How do I use this?

To start with, get the appropriate [NetCore 5 SDK (With AspNet)](https://dotnet.microsoft.com/download/dotnet/5.0).

You're going to need to make a new Twitch account for the bot, if it doesn't already exist.
You also need to go to [The Twitch Dev Console](https://dev.twitch.tv/console/apps) and register an application to receive a ClientID.
Enter any name, use `http://localhost:5000/TASagentBotAPI/OAuth/BotCode` and `http://localhost:5000/TASagentBotAPI/OAuth/BroadcasterCode` as the OAuth Redirect URLs, and choose "Chat Bot" as the category.