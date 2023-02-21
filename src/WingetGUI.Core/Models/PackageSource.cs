namespace WingetGUI.Core.Models
{
    public class PackageSource : IPackage
    {
        public string Name { get; init; } = "";

        public string Url { get; init; } = "";

        public string Type { get; init; } = "";

        public bool HasValue => !string.IsNullOrWhiteSpace(Name);
    }
}
