using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add the .NET 'gitignore' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new .gitignore file.</param>
    /// <returns></returns>
    public static async Task AddGitIgnoreAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"- 📄 Adding '.gitignore' to project root... ", false);

        if (File.Exists(Path.Combine(outputDirectory, ".gitignore")))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
            {
                File.Delete(Path.Combine(outputDirectory, ".gitignore"));
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
                "gitignore"
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