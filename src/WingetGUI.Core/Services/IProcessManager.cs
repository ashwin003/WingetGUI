using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services
{
    /// <summary>
    /// <see cref="internal"/> class which provides a wrapper around <see cref="Process"/>
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        /// Starts a new process with the given parameters and returns the result as an instance of <see cref="ProcessOutput"/> class.
        /// </summary>
        /// <param name="command">The process to be started</param>
        /// <param name="arguments">The arguments to be passed to the process</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns>Output of the executed process</returns>
        Task<ProcessOutput> ExecuteAsync(string command, string arguments = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts a new process with the given parameters
        /// </summary>
        /// <param name="command">The process to be started</param>
        /// <param name="arguments">The arguments to be passed to the process</param>
        /// <param name="onDataReceived">A callback which gets fired whenever new data is available from the process</param>
        /// <param name="onErrorReceived">A callback which gets fired whenever the process throws an error</param>
        /// <param name="cancellationToken">A handle to cancel execution</param>
        /// <returns></returns>
        Task StreamAsync(string command, string arguments, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default);
    }
}
