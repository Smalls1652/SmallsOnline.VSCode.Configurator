using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add a project to a .NET solution.
    /// </summary>
    /// <param name="solutionFilePath">The path to the solution file.</param>
    /// <param name="projectPath">The path to the project.</param>
    /// <returns></returns>
    /// <exception cref="Exception">An error occurred while adding the project to the solution.</exception>
    public static async Task AddProjectToSolutionAsync(string solutionFilePath, string projectPath)
    {
        string solutionFilePathRelative = Path.GetRelativePath(Directory.GetCurrentDirectory(), solutionFilePath);
        string projectPathRelative = Path.GetRelativePath(Directory.GetCurrentDirectory(), projectPath);

        ConsoleUtils.WriteInfo($"- 📄 Adding '{projectPathRelative}' to solution '{solutionFilePathRelative}'... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "sln",
                solutionFilePath,
                "add",
                projectPath
            ],
            workingDirectory: Path.GetDirectoryName(solutionFilePath) ?? throw new Exception("Failed to get solution directory.")
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet sln add' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add project '{projectPathRelative}' to solution '{solutionFilePathRelative}':\n\n{dotnetNewErrorText}");
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