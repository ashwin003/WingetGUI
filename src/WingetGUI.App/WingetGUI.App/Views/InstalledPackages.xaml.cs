using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WingetGUI.App.ViewModels;

namespace WingetGUI.App.Views
{
    public sealed partial class InstalledPackages : Page
    {
        private readonly InstalledPackagesViewModel viewModel;
        public InstalledPackages()
        {
            this.InitializeComponent();
            viewModel = App.GetService<InstalledPackagesViewModel>();
            this.DataContext = viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await viewModel.LoadPackagesAsync();
        }
    }
}