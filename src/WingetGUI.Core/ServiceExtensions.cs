using WingetGUI.Core.Services;
using WingetGUI.Core.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddWingetServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IProcessManager, ProcessManager>()
                .AddScoped<IPackageSourceManager, WingetSourceManager>()
                .AddScoped<IPackageManager, WingetPackageManager>()
                .AddScoped<IFeaturesService, WingetFeaturesService>();
        }
    }
}