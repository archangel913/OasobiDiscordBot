namespace Application.Settings
{
    public record GuildId
    {
        public GuildId(ulong? value)
        {
            if (value is null) throw new KeyNotFoundException("Not found guildId element in App.config");
            this.Value = (ulong)value;
        }

        public ulong Value { get; }
    }
}