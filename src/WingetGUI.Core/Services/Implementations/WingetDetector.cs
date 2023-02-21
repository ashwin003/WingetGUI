using WingetGUI.Core.Helpers;

namespace WingetGUI.Core.Services.Implementations
{
    internal class WingetDetector : IWingetDetector
    {
        private readonly IProcessManager processManager;

        public WingetDetector(IProcessManager processManager)
        {
            this.processManager = processManager;
        }

        public async Task<bool> DetectAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, cancellationToken: cancellationToken);
            return processOutput?.IsSuccess ?? false;
        }

        public async Task<string> FetchInstalledVertionAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, "--version", cancellationToken);
            return processOutput.Output.First();
        }
    }
}
