using Microsoft.Extensions.DependencyInjection;
using Domain.Interface;
using Application.Interface;
using DiscordUI.BotConsole;
using DiscordUI.Modules.ExperimentalModules;
using DiscordUI.Modules.MusicModule;

namespace DiscordUI
{
    public class Services
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IConsoleReader, ConsoleReader>();
        }
    }
}
