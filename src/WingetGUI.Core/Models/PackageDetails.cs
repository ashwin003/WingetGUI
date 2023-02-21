namespace WingetGUI.Core.Models
{
    public class PackageDetails : IPackage
    {
        public string Version { get; init; } = "";

        public string Name { get; init; } = "";

        public string Description { get; init; } = "";

        public string Publisher { get; init; } = "";

        public string PublisherUrl { get; init; } = "";

        public string PublisherSupport { get; init; } = "";

        public string Author { get; init; } = "";

        public string Moniker { get; init; } = "";

        public string Homepage { get; init; } = "";

        public string License { get; init; } = "";

        public string LicenseUrl { get; init; } = "";

        public string PrivacyUrl { get; init; } = "";

        public string Copyright { get; init; } = "";

        public string CopyrightUrl { get; init; } = "";

        public IReadOnlyList<string> Tags { get; init; } = new List<string>();

        public PackageInstaller Installer { get; init; } = new PackageInstaller();

        public bool HasValue => !string.IsNullOrWhiteSpace(Version);
    }
}
