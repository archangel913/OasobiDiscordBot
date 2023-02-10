using Domain.Interface;
using Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Discord;
using Infrastructure.LocalFile;
using Infrastructure.VideoLibrary;
using Application.Settings;
using Infrastructure.Loggings;

namespace Infrastructure
{
    public class Services
    {
        public void RegisterServices(IServiceCollection services, BotSettings settings)
        {
            services.AddTransient<IDiscordConnecter, Connecter>();
            services.AddTransient<IFileReader,FileReader>();
            services.AddTransient<IAudioSender, AudioSender>();
            services.AddTransient<IHttp, Http.Http>();
            services.AddTransient<IVideoLib, VideoLib>();
            services.AddSingleton<ISettingsReader, SettingsReader>();
            services.AddSingleton<ILanguageRepository, LanguageRepository>(factory =>
            {
                return new LanguageRepository(settings);
            });
            var fileWriter = new FileWriter();
            services.AddTransient<IFileWriter, FileWriter>(value => fileWriter);
            services.AddSingleton<IDiscordLogger, DiscordLogger>(value => new DiscordLogger(fileWriter));
        }
    }
}
