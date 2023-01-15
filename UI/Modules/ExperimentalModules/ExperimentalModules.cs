using Application.Exprimental;
using Discord.Interactions;
using Discord.WebSocket;
using Application.Languages;
using Application.Interface;
using System.Reflection;

namespace UI.Modules.ExperimentalModules
{
    public class ExperimentalModules : InteractionModuleBase, IAssembleGetable
    {
        [SlashCommand("test", "test")]
        public async Task Test()
        {
            try
            {
                LanguageDictionary languageDictionary = Experimental.TestLang();
                await this.ReplyAsync(languageDictionary["fat"]);
                await this.RespondAsync(languageDictionary["tenno"]);
            }
            catch(Exception e)
            {
                await RespondAsync(e.Message);
                throw;
            }
        }

        public Assembly? GetAssembly()
        {
            return Assembly.GetAssembly(typeof(ExperimentalModules));
        }
    }
}
