using Domain.Interface;
using Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Discord;
using Infrastructure.LocalFile;
using Infrastructure.VideoLibrary;

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
            services.AddTransient<IVideoLib, VideoLib>();
            services.AddTransient<IFileWriter, FileWriter>();
            services.AddSingleton<ISettingsReader, SettingsReader>();
            services.AddSingleton<ILanguageRepository, LanguageRepository>();
        }
    }
}
