using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

/// <summary>
/// A collection of methods for running operations with the 'git' CLI.
/// </summary>
public static class GitOperations
{
    /// <summary>
    /// Create a new <see cref="ProcessStartInfo"/> to run a 'git' process.
    /// </summary>
    /// <param name="arguments">Arguments to pass to the 'git' process.</param>
    /// <param name="workingDirectory">The working directory for the 'git' process.</param>
    /// <returns>A new <see cref="ProcessStartInfo"/> instance.</returns>
    private static ProcessStartInfo CreateGitProcessStartInfo(string[] arguments, string workingDirectory)
    {
        return new(
            fileName: "git",
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
    /// Initialize a new Git repository.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new repository.</param>
    /// <returns></returns>
    public static async Task InitializeGitRepositoryAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo("\n📦 Initializing Git repository... ", false);

        ProcessStartInfo gitInitProcessStartInfo = CreateGitProcessStartInfo(
            arguments: [
                "init"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process gitInitProcess = Process.Start(gitInitProcessStartInfo) ?? throw new Exception("Failed to start 'git init' process.");
            
            await gitInitProcess.WaitForExitAsync();

            if (gitInitProcess.ExitCode != 0)
            {
                string gitInitErrorText = await gitInitProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to initialize Git repository:\n\n{gitInitErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }
}