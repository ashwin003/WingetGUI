using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingetGUI.App.Models;
using WingetGUI.Core.Services;

namespace WingetGUI.App.ViewModels
{
    public sealed partial class UpdatesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IEnumerable<PackageToUpgrade> upgradeablePackages = new List<PackageToUpgrade>();

        private readonly IPackageManager packageManager;

        public UpdatesViewModel(IPackageManager packageManager)
        {
            this.packageManager = packageManager;
        }

        public async Task LoadPackagesAsync()
        {
            IsLoading = true;
            var packages = await Task.Run(async () => await packageManager.FetchUpgradablePackages(false));
            UpgradeablePackages = packages.Select(package => new PackageToUpgrade { Id = package.Id, AvailableVersion = package.AvailableVersion, InstalledVersion = package.InstalledVersion, Name = package.Name, Source = package.Source, Version = package.Version, IsSelected = false });
            IsLoading = false;
        }
    }
}
