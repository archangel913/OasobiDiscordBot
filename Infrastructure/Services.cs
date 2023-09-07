using Domain.Interface;
using Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Discord;
using Infrastructure.LocalFile;
using Infrastructure.Videos;
using Infrastructure.YouTubeMusics;
using Application.Settings;
using Infrastructure.Loggings;
using Application.Bots;
using System.Reflection;

namespace Infrastructure
{
    public class Services
    {
        public void RegisterServices(IServiceCollection services, BotSettings settings, IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IAudioSender, AudioSender>();
            services.AddTransient<IVideoLib, Video>();
            services.AddTransient<IFileRepository, FileRepository>();
            var musicGetter = new GetMusic(settings.YouTubeToken);
            services.AddSingleton<IGetMusic, GetMusic>(factry =>
            {
                return musicGetter;
            });
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            services.AddSingleton<ILanguageRepository, LanguageRepository>(factory =>
            {
                return new LanguageRepository(settings);
            });
            var fileRepository = new FileRepository();
            services.AddTransient<IFileRepository, FileRepository>(value => fileRepository);
            services.AddSingleton<IDiscordLogger, DiscordLogger>(value => new DiscordLogger(fileRepository));

            var bot = new BotClient(settings, services);
            bot.SetModulesAsync(assemblies).Wait();
            services.AddSingleton<IAsyncBotClient, BotClient>(factory =>
            {
                return bot;
            });
        }
    }
}
