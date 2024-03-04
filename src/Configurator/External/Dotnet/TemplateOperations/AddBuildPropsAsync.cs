using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Add the .NET 'build.props' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new build.props file.</param>
    /// <returns></returns>
    public static async Task AddBuildPropsAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"- 📄 Adding 'build.props' to project root... ", false);

        if (File.Exists(Path.Combine(outputDirectory, "Directory.Build.props")))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
                {
                    File.Delete(Path.Combine(outputDirectory, "Directory.Build.props"));
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
                "buildprops",
                "--use-artifacts"
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