using Application.Settings;

namespace Application.Interface
{
    public interface ISettingsReader
    {
        public bool TryGetSettings(out BotSettings result);
    }
}
