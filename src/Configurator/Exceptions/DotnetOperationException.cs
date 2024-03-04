using System.Diagnostics;

namespace SmallsOnline.VSCode.Configurator;

internal class DotnetOperationException : Exception
{
    public DotnetOperationException(string? message, ProcessStartInfo processStartInfo) : base(message)
    {
        StartInfo = processStartInfo;
    }

    public DotnetOperationException(string? message, ProcessStartInfo processStartInfo, string processErrorText) : this(message, processStartInfo)
    {
        ProcessErrorText = processErrorText;
    }

    public DotnetOperationException(string? message, ProcessStartInfo processStartInfo, Exception? innerException) : base(message, innerException)
    {
        StartInfo = processStartInfo;
    }

    public DotnetOperationException(string? message, ProcessStartInfo processStartInfo, string processErrorText, Exception? innerException) : this(message, processStartInfo, innerException)
    {
        ProcessErrorText = processErrorText;
    }

    public ProcessStartInfo StartInfo { get; set; }

    public string? ProcessErrorText { get; set; }
}
