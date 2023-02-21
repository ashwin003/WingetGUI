namespace WingetGUI.Core.Models
{
    public class ProcessOutput
    {
        public int ExitCode { get; init; }

        public IReadOnlyList<string> Output { get; init; } = new List<string>();

        public bool IsSuccess { get => ExitCode is 0; }
    }
}
