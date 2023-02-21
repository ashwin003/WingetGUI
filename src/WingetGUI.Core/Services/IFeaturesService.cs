using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services
{
    /// <summary>
    /// Handles experimental features related to winget.
    /// </summary>
    public interface IFeaturesService
    {
        /// <summary>
        /// Fetches all experimental features present in the system regardless of whether or not they are enabled.
        /// </summary>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>List of winget features.</returns>
        Task<IReadOnlyList<Feature>> FetchFeaturesAsync(CancellationToken cancellationToken = default);
    }
}
