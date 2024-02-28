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

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "gitignore"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new gitignore' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add .gitignore:\n\n{dotnetNewErrorText}");
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