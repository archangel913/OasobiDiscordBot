namespace Application.Settings
{
    public record Version
    {
        public Version() { }

        public Version(string version)
        {
            var splitedVersion = version.Split('.');
            if (splitedVersion.Length != 3 ||
                !int.TryParse(splitedVersion[0], out int major) ||
                !int.TryParse(splitedVersion[1], out int minor) ||
                !int.TryParse(splitedVersion[2], out int patch))
            {
                throw new FormatException();
            }

            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
        }

        public int Major { get; init; }

        public int Minor { get; init; }

        public int Patch { get; init; }
    }
}