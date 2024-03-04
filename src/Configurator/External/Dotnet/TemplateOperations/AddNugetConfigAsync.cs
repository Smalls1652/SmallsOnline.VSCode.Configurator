using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add the .NET 'NuGet.Config' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new NuGet.Config file.</param>
    /// <returns></returns>
    public static async Task AddNugetConfigAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"- 📄 Adding 'NuGet.Config' to project root... ", false);

        if (File.Exists(Path.Combine(outputDirectory, "NuGet.Config")) || File.Exists(Path.Combine(outputDirectory, "nuget.config")))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
                {
                    File.Delete(Path.Combine(outputDirectory, "NuGet.Config"));
                }
                else
                {
                    ConsoleUtils.WriteWarning("Already exists. 🟠\n", false);
                    return;
                }
        }

        ProcessStartInfo processStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "nugetconfig"
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