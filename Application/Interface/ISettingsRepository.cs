using Application.Settings;

namespace Application.Interface
{
    public interface ISettingsRepository
    {
        public bool TryGetSettings(out BotSettings result);

        public void Save(BotSettings newBotSettings);
    }
}
