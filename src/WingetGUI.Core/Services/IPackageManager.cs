using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services
{
    /// <summary>
    /// Handles winget packages
    /// </summary>
    public interface IPackageManager
    {
        /// <summary>
        /// Fetches the list of installed packages using winget CLI
        /// </summary>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>A readonly <see cref="List{T}"/> of <see cref="InstalledPackage"/> instances</returns>
        Task<IReadOnlyList<InstalledPackage>> FetchInstalledPackagesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Searches for packages using the given search term
        /// </summary>
        /// <param name="searchTerm">The term to search</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>A readonly <see cref="List{T}"/> of <see cref="Package"/> instances</returns>
        Task<IReadOnlyList<Package>> SearchPackages(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Installs the specified package using winget
        /// </summary>
        /// <param name="packageId">Id of the package to be installed</param>
        /// <param name="onDataReceived">Callback for receiving data from the installation process</param>
        /// <param name="onErrorReceived">Callback for receiving error data from the installation process</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns></returns>
        Task InstallAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uninstalls the specified package using winget
        /// </summary>
        /// <param name="packageId">Id of the package to be uninstalled</param>
        /// <param name="onDataReceived">Callback for receiving data from the installation process</param>
        /// <param name="onErrorReceived">Callback for receiving error data from the installation process</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successful, <see langword="false"/> otherwise</returns>
        Task UninstallAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches a list of packages which can be upgraded
        /// </summary>
        /// <param name="includeUnknown">Include packages with unknown version numbers</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>A readonly <see cref="List{T}"/> of <see cref="Package"/> instances</returns>
        Task<IReadOnlyList<UpgradeablePackage>> FetchUpgradablePackages(bool includeUnknown, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upgrades the specified package using winget
        /// </summary>
        /// <param name="packageId">Id of the package to be upgraded</param>
        /// <param name="onDataReceived">Callback for receiving data from the installation process</param>
        /// <param name="onErrorReceived">Callback for receiving error data from the installation process</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if the action was successful, <see langword="false"/> otherwise</returns>
        Task UpgradeAsync(string packageId, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches the available details for the specified package
        /// </summary>
        /// <param name="packageId">Id of the package whose details are to be retrieved</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>The details of the specified package as an instance of the <see cref="PackageDetails"/> class</returns>
        Task<PackageDetails> FetchPackageDetailsAsync(string packageId, CancellationToken cancellationToken = default);
    }
}
