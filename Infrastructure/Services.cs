using Domain.Interface;
using Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Discord;
using Infrastructure.LocalFile;
using Infrastructure.Videos;
using Infrastructure.YouTubeMusics;

namespace Infrastructure
{
    public class Services
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IDiscordConnecter, Connecter>();
            services.AddTransient<IFileReader,FileReader>();
            services.AddTransient<IAudioSender, AudioSender>();
            services.AddTransient<IHttp, Http.Http>();
            services.AddTransient<IVideoLib, Video>();
            services.AddTransient<IFileWriter, FileWriter>();
            services.AddTransient<IGetMusic, GetMusic>();
            services.AddSingleton<ISettingsReader, SettingsReader>();
            services.AddSingleton<ILanguageRepository, LanguageRepository>();

        }
    }
}
