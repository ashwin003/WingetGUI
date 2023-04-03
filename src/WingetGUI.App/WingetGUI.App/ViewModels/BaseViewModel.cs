using CommunityToolkit.Mvvm.ComponentModel;

namespace WingetGUI.App.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading;
    }
}
