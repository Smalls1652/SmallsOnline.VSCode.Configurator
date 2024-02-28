using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class DotnetOperations
{
    /// <summary>
    /// Initialize a new .NET tool manifest.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new tool manifest.</param>
    /// <returns></returns>
    public static async Task InitializeDotnetToolManifestAsync(string outputDirectory)
    {
        if (File.Exists(Path.Combine(outputDirectory, ".config", "dotnet-tools.json")))
        {
            return;
        }

        ConsoleUtils.WriteInfo($"- 📦 Initializing .NET tool manifest... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "tool-manifest"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new tool-manifest' process.");

            await ConsoleUtils.WriteProgressIndicatorAsync(dotnetNewProcess.WaitForExitAsync(), Console.GetCursorPosition());

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to initialize .NET tool manifest:\n\n{dotnetNewErrorText}");
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
