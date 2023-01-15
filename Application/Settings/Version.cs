namespace Application.Settings
{
    public record Version
    {
        public int Major { get; init; }

        public int Minor { get; init; }

        public int Patch { get; init; }
    }
}