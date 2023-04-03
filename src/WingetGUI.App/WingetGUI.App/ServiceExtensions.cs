using WingetGUI.App.ViewModels;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services.AddSingleton<UpdatesViewModel>().AddSingleton<InstalledPackagesViewModel>();
        }
    }
}
