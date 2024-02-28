using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add the .NET 'global.json' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new global.json file.</param>
    /// <returns></returns>
    public static async Task AddGlobalJsonAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"- 📄 Adding 'global.json' to project root... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "globaljson",
                "--roll-forward",
                "latestMinor"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new globaljson' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add global.json:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}