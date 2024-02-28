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

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
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
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new sln' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to initialize .NET solution '{solutionName}.sln':\n\n{dotnetNewErrorText}");
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
