using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

/// <summary>
/// A collection of methods for running operations with the 'dotnet' CLI.
/// </summary>
public static partial class DotnetOperations
{
    /// <summary>
    /// Create a new <see cref="ProcessStartInfo"/> to run a 'dotnet' process.
    /// </summary>
    /// <param name="arguments">Arguments to pass to the 'dotnet' process.</param>
    /// <param name="workingDirectory">The working directory for the 'dotnet' process.</param>
    /// <returns>A new <see cref="ProcessStartInfo"/> instance.</returns>
    private static ProcessStartInfo CreateDotnetProcessStartInfo(string[] arguments, string workingDirectory)
    {
        return new(
            fileName: "dotnet",
            arguments: arguments
        )
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };
    }

    /// <summary>
    /// Run a 'dotnet' process asynchronously.
    /// </summary>
    /// <param name="processStartInfo">The <see cref="ProcessStartInfo"/> for the 'dotnet' process.</param>
    /// <returns></returns>
    /// <exception cref="Exception">An error occurred while running the 'dotnet' process.</exception>
    private static async Task RunDotnetProcessAsync(ProcessStartInfo processStartInfo)
    {
        using Process dotnetProcess = Process.Start(processStartInfo) ?? throw new DotnetOperationException("Failed to start 'dotnet' process.", processStartInfo);

        await dotnetProcess.WaitForExitAsync();

        if (dotnetProcess.ExitCode != 0)
        {
            string dotnetErrorText = await dotnetProcess.StandardError.ReadToEndAsync();

            throw new DotnetOperationException(
                message: $"An error occurred while running 'dotnet {string.Join(' ', processStartInfo.ArgumentList)}'.",
                processStartInfo: processStartInfo,
                processErrorText: dotnetErrorText
            );
        }
    }
}