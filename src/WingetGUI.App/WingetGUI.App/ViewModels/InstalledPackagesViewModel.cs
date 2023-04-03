using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using WingetGUI.Core.Models;
using WingetGUI.Core.Services;

namespace WingetGUI.App.ViewModels
{
    public partial class InstalledPackagesViewModel : BaseViewModel
    {
        private readonly IPackageManager packageManager;

        [ObservableProperty]
        private IEnumerable<InstalledPackage> installedPackages = new List<InstalledPackage>();

        public InstalledPackagesViewModel(IPackageManager packageManager)
        {
            this.packageManager = packageManager;
        }

        public async Task LoadPackagesAsync()
        {
            IsLoading = true;
            InstalledPackages = await Task.Run(async () => await packageManager.FetchInstalledPackagesAsync());
            IsLoading = false;
        }
    }
}
