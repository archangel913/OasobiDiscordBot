using Microsoft.Extensions.DependencyInjection;
using Domain.Interface;
using Application.Interface;
using UI.BotConsole;
using UI.Modules.ExperimentalModules;
using UI.Modules.MusicModule;

namespace UI
{
    public class Services
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IConsoleReader, ConsoleReader>();
            services.AddTransient<IDiscordLogger, DiscordLogger>();
            services.AddTransient<IAssembleGetable, ExperimentalModules>();
            services.AddTransient<IAssembleGetable, MusicModule>();
        }
    }
}
