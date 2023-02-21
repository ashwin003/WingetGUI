using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services
{
    /// <summary>
    /// Manages sources used by Winget
    /// </summary>
    public interface IPackageSourceManager
    {
        /// <summary>
        /// Fetches the list of all installed sources at winget
        /// </summary>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>List of all sources at nuget as a list of <see cref="PackageSource"/></returns>
        Task<IReadOnlyList<PackageSource>> FetchInstalledSourcesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new source to winget
        /// </summary>
        /// <remarks>Needs elevated privileges</remarks>
        /// <param name="source">The source to be added</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successfull, <see langword="false"/> otherwise</returns>
        Task<bool> RegisterSourceAsync(PackageSource source, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates all sources that are installed in winget
        /// </summary>
        /// <remarks>Might take a while</remarks>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successfull, <see langword="false"/> otherwise</returns>
        Task<bool> UpdateSourcesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Resets all sources installed in winget
        /// </summary>
        /// <remarks>Needs elevated privileges</remarks>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successfull, <see langword="false"/> otherwise</returns>
        Task<bool> ResetSourcesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified source from winget
        /// </summary>
        /// <remarks>Needs elevated privileges</remarks>
        /// <param name="name">Name of the source to be removed</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successfull, <see langword="false"/> otherwise</returns>
        Task<bool> RemoveSourceAsync(string name, CancellationToken cancellationToken = default);
    }
}
