using CliWrap;
using CliWrap.Buffered;
using System.Diagnostics;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services.Implementations
{
    internal class ProcessManager : IProcessManager
    {
        public async Task<ProcessOutput> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken)
        {
            try
            {
                var response = await Cli.Wrap(command).WithArguments(arguments).ExecuteBufferedAsync(cancellationToken);
                var output = response.StandardOutput.Split(Environment.NewLine).Select(r => r.Split("\r").Last());
                return new ProcessOutput { ExitCode = response.ExitCode, Output = output.ToList() };
            }
            catch(Exception)
            {
                return new ProcessOutput { ExitCode = -1 };
            }
        }

        public async Task StreamAsync(string command, string arguments, Action<string> onDataReceived, Action<string> onErrorReceived, CancellationToken cancellationToken = default)
        {
            var process = PrepareProcess(command, arguments);

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data is not null) onDataReceived(e.Data);
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data is not null) onErrorReceived(e.Data);
            };
            cancellationToken.Register(() => { process.Kill(); process.Dispose(); });

            var isStarted = process.Start();
            if (isStarted)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            await Task.CompletedTask;
        }

        private static Process PrepareProcess(string command, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(command, arguments)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            return new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };
        }
    }
}
