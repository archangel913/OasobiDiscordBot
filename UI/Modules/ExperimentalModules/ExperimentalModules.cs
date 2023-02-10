using Application.Exprimental;
using Discord.Interactions;
using Discord.WebSocket;
using Application.Languages;
using Application.Interface;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interface;

namespace UI.Modules.ExperimentalModules
{
    public class ExperimentalModules : ModuleBase, IAssembleGetable
    {
        public ExperimentalModules(IServiceProvider services) : base(services)
        {
        }

        [SlashCommand("test", "test")]
        public async Task Test()
        {
            var logger = this.Services.GetRequiredService<IDiscordLogger>();
            await this.RespondAsync(logger.ToString());
        }

        public Assembly? GetAssembly()
        {
            return Assembly.GetAssembly(typeof(ExperimentalModules));
        }
    }
}
