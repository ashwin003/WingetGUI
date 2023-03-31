namespace WingetGUI.Core.Models
{
    public class Package : IPackage
    {
        public string Name { get; init; } = "";

        public string Id { get; init; } = "";

        public string Version { get; init; } = "";

        public string Source { get; init; } = "";

        public bool HasValue => !string.IsNullOrWhiteSpace(Id);
    }

    public class InstalledPackage : Package
    {
        public string InstalledVersion { get; init; } = "";

        public string AvailableVersion { get; init; } = "";
    }

    public class UpgradeablePackage : InstalledPackage
    {
    }
}
