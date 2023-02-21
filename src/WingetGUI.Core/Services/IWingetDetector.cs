namespace WingetGUI.Core.Services
{
    /// <summary>
    /// Handles winget installation in the system
    /// </summary>
    public interface IWingetDetector
    {
        /// <summary>
        /// Checks whether or not winget is installed on the system
        /// </summary>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns><see langword="true"/> if winget is installed, <see langword="false"/> otherwise</returns>
        Task<bool> DetectAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches the version of winget that is installed
        /// </summary>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>Installed winget version</returns>
        Task<string> FetchInstalledVertionAsync(CancellationToken cancellationToken = default);
    }
}
