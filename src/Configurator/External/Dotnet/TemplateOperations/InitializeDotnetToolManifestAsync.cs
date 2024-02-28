using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Initialize a new .NET tool manifest.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new tool manifest.</param>
    /// <returns></returns>
    public static async Task InitializeDotnetToolManifestAsync(string outputDirectory)
    {
        if (File.Exists(Path.Combine(outputDirectory, ".config", "dotnet-tools.json")))
        {
            return;
        }

        ConsoleUtils.WriteInfo($"- 📦 Initializing .NET tool manifest... ", false);

        ProcessStartInfo processStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "tool-manifest"
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
