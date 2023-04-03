using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WingetGUI.App.ViewModels;

namespace WingetGUI.App.Views
{
    public sealed partial class Updates : Page
    {
        private readonly UpdatesViewModel viewModel;
        public Updates()
        {
            this.InitializeComponent();
            viewModel = App.GetService<UpdatesViewModel>();
            this.DataContext = viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.viewModel.LoadPackagesAsync();
        }
    }
}