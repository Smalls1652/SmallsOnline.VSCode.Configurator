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

        ProcessStartInfo processStartInfo = CreateDotnetProcessStartInfo(
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