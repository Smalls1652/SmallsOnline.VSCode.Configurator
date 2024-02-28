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

        ProcessStartInfo processStartInfo = CreateDotnetProcessStartInfo(
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