using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TASagentTwitchBot.IRCOnly
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddSingleton<IRCOnlyApplication>()
                .AddSingleton<Core.ErrorHandler>()
                .AddSingleton<Core.ApplicationManagement>()
                .AddSingleton<Core.IRC.IrcClient>()
                .AddSingleton<Core.Chat.ChatLogger>()
                .AddSingleton<Core.API.Twitch.HelixHelper>();

            services
                .AddSingleton<ButtonPresser.ChatListener>();
            
            services
                .AddSingleton<Core.Config.IExternalWebAccessConfiguration, Config.ExternalWebAccessConfiguration>()
                .AddSingleton<Core.Chat.IChatMessageHandler, Chat.ChatMessageHandler>()
                .AddSingleton<Core.API.Twitch.IBotTokenValidator, Core.API.Twitch.BotTokenValidator>()
                .AddSingleton<Core.API.Twitch.IBroadcasterTokenValidator, Core.API.Twitch.BroadcasterTokenValidator>()
                .AddSingleton<Core.IRC.INoticeHandler, IRC.PointlessNoticeHandler>()
                .AddSingleton<Core.IRC.IIRCLogger, Core.IRC.IRCLogger>()
                .AddSingleton<Core.ICommunication, Core.CommunicationHandler>()
                .AddSingleton<Core.Config.IBotConfigContainer, Core.Config.BotConfigContainer>()
                .AddSingleton<Core.View.IConsoleOutput, Core.View.BasicView>();

            services
                .AddSingleton<ButtonPresser.IButtonPressDispatcher, ButtonPresser.ButtonPressDispatcher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ApplicationServices.GetRequiredService<Core.Config.IBotConfigContainer>().Initialize();
            app.ApplicationServices.GetRequiredService<Core.View.IConsoleOutput>();
            app.ApplicationServices.GetRequiredService<Core.Chat.ChatLogger>();
            app.ApplicationServices.GetRequiredService<ButtonPresser.ChatListener>();
        }
    }
}
