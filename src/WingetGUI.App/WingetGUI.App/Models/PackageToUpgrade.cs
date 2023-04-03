using WingetGUI.Core.Models;

namespace WingetGUI.App.Models
{
    public sealed class PackageToUpgrade : UpgradeablePackage
    {
        public bool IsSelected { get; set; }
    }
}
