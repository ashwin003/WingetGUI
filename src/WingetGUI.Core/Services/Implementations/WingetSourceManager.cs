using WingetGUI.Core.Extensions;
using WingetGUI.Core.Helpers;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services.Implementations
{
    internal class WingetSourceManager : IPackageSourceManager
    {
        private readonly IProcessManager processManager;

        public WingetSourceManager(IProcessManager processManager)
        {
            this.processManager = processManager;
        }
        public async Task<IReadOnlyList<PackageSource>> FetchInstalledSourcesAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, "source list", cancellationToken);

            return processOutput.ToPackageSources();
        }

        public async Task<bool> RegisterSourceAsync(PackageSource source, CancellationToken cancellationToken = default)
        {
            if (!source.HasValue) return false;
            var processArguments = string.IsNullOrWhiteSpace(source.Type) ?
                $"source add -n {source.Name} -a {source.Url} --accept-source-agreements" :
                $"source add -n {source.Name} -a {source.Url} -t {source.Type} --accept-source-agreements";
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, processArguments, cancellationToken);
            return CheckSuccess(processOutput);
        }

        public async Task<bool> RemoveSourceAsync(string name, CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, $"source remove -n {name}", cancellationToken);
            return CheckSuccess(processOutput);
        }

        public async Task<bool> ResetSourcesAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, "source reset --force", cancellationToken);
            return CheckSuccess(processOutput);
        }

        public async Task<bool> UpdateSourcesAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, "source update", cancellationToken);
            return CheckSuccess(processOutput);
        }

        private static bool CheckSuccess(ProcessOutput processOutput)
        {
            return processOutput.IsSuccess;
        }
    }
}
