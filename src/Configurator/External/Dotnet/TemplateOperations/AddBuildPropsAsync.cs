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

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "buildprops",
                "--use-artifacts"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new buildprops' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add build.props:\n\n{dotnetNewErrorText}");
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