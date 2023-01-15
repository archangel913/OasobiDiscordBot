using Application.Settings;

namespace Application.Interface
{
    public interface ISettingsReader
    {
        public BotSettings GetSettings();
    }
}
