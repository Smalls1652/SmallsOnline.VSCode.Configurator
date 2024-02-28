using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add a .NET tool to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new tool manifest.</param>
    /// <param name="toolName">The name of the tool to add.</param>
    /// <returns></returns>
    public static async Task AddDotnetToolAsync(string outputDirectory, string toolName)
    {
        await InitializeDotnetToolManifestAsync(outputDirectory);

        ConsoleUtils.WriteInfo($"- 📦 Adding .NET tool '{toolName}'... ", false);

        ProcessStartInfo processStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "tool",
                "install",
                toolName,
                "--tool-manifest",
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            await ConsoleUtils.WriteProgressIndicatorAsync(RunDotnetProcessAsync(processStartInfo), Console.GetCursorPosition());
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}
