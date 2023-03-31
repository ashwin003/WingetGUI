using WingetGUI.Core.Extensions;
using WingetGUI.Core.Helpers;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services.Implementations
{
    internal class WingetPackageManager : IPackageManager
    {
        private readonly IProcessManager processManager;

        public WingetPackageManager(IProcessManager processManager)
        {
            this.processManager = processManager;
        }

        public async Task<IReadOnlyList<Package>> SearchPackages(string searchTerm, CancellationToken cancellationToken)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, $"search {searchTerm} --accept-source-agreements", cancellationToken);
            return processOutput.ToPackages(0);
        }

        public async Task<IReadOnlyList<UpgradeablePackage>> FetchUpgradablePackages(bool includeUnknown, CancellationToken cancellationToken = default)
        {
            var argument = includeUnknown ? "upgrade --include-unknown" : "upgrade";
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, argument, cancellationToken);
            return processOutput.ToUpgradeablePackages(0);
        }

        public async Task InstallAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default)
        {
            await processManager.StreamAsync(Constants.WingetProcessName, $"install {packageId}", onDataReceived, onErrorReceived, cancellationToken);
        }

        public async Task UninstallAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default)
        {
            await processManager.StreamAsync(Constants.WingetProcessName, $"uninstall {packageId}", onDataReceived, onErrorReceived, cancellationToken);
        }

        public async Task UpgradeAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default)
        {
            await processManager.StreamAsync(Constants.WingetProcessName, $"upgrade {packageId}", onDataReceived, onErrorReceived, cancellationToken);
        }

        public async Task<PackageDetails> FetchPackageDetailsAsync(string packageId, CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, $"show {packageId}", cancellationToken);
            return processOutput.ToPackageDetails();
        }
    }
}
