using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Initialize a new .NET solution.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new solution.</param>
    /// <param name="solutionName">The name of the new solution.</param>
    /// <returns></returns>
    public static async Task InitializeDotnetSolutionAsync(string outputDirectory, string solutionName)
    {
        ConsoleUtils.WriteInfo($"- 📦 Initializing .NET solution '{solutionName}.sln'... ", false);

        if (File.Exists(Path.Combine(outputDirectory, $"{solutionName}.sln")))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
            {
                File.Delete(Path.Combine(outputDirectory, $"{solutionName}.sln"));
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
                "sln",
                "--name",
                solutionName
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
