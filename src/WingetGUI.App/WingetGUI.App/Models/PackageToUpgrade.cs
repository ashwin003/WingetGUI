using WingetGUI.Core.Models;

namespace WingetGUI.App.Models
{
    public class PackageToUpgrade : UpgradeablePackage
    {
        public bool IsSelected { get; set; }
    }
}
