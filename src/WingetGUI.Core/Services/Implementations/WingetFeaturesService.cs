using WingetGUI.Core.Extensions;
using WingetGUI.Core.Helpers;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services.Implementations
{
    internal class WingetFeaturesService : IFeaturesService
    {
        private readonly IProcessManager processManager;

        public WingetFeaturesService(IProcessManager processManager)
        {
            this.processManager = processManager;
        }

        public async Task<IReadOnlyList<Feature>> FetchFeaturesAsync(CancellationToken cancellationToken = default)
        {
            var processOutput = await processManager.ExecuteAsync(Constants.WingetProcessName, "features", cancellationToken);
            return processOutput.ToFeatures(2);
        }
    }
}
