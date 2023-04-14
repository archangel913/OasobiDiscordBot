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
        public IServiceProvider RegisterServices(IServiceCollection services, BotSettings settings, IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<IFileRepository,FileRepository>();
            services.AddTransient<IAudioSender, AudioSender>();
            services.AddTransient<IHttp, Http.Http>();
            services.AddTransient<IVideoLib, Video>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IGetMusic, GetMusic>();
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
            return services.BuildServiceProvider();
        }
    }
}
