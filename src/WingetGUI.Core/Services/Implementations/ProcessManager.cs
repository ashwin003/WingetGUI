using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Services.Implementations
{
    internal class ProcessManager : IProcessManager
    {
        public Task<ProcessOutput> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken)
        {
            var process = PrepareProcess(command, arguments);
            var tcs = new TaskCompletionSource<ProcessOutput>();

            process.Exited += async (s, e) =>
            {
                await HandleProcessResponse(tcs, process);
            };
            cancellationToken.Register(() => { process.Kill(); process.Dispose(); });

            try
            {
                process.Start();
            }
            catch (Win32Exception)
            {
                tcs.SetResult(new ProcessOutput { ExitCode = -1 });
            }

            return tcs.Task;
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


        private static async Task HandleProcessResponse(TaskCompletionSource<ProcessOutput> tcs, Process process)
        {
            var output = new ConcurrentBag<string>();
            using var processOutputStream = process.StandardOutput;
            while (!processOutputStream.EndOfStream)
            {
                var line = await processOutputStream.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line)) continue;

                output.Add(line);
            }

            tcs.SetResult(new ProcessOutput { ExitCode = process.ExitCode, Output = output.ToList() });
            process.Dispose();
        }
    }
}
