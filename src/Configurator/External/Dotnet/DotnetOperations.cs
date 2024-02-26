using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

/// <summary>
/// A collection of methods for running operations with the 'dotnet' CLI.
/// </summary>
public static partial class DotnetOperations
{
    /// <summary>
    /// Create a new <see cref="ProcessStartInfo"/> to run a 'dotnet' process.
    /// </summary>
    /// <param name="arguments">Arguments to pass to the 'dotnet' process.</param>
    /// <param name="workingDirectory">The working directory for the 'dotnet' process.</param>
    /// <returns>A new <see cref="ProcessStartInfo"/> instance.</returns>
    private static ProcessStartInfo CreateDotnetProcessStartInfo(string[] arguments, string workingDirectory)
    {
        return new(
            fileName: "dotnet",
            arguments: arguments
        )
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };
    }

    /// <summary>
    /// Run a 'dotnet' process asynchronously.
    /// </summary>
    /// <param name="processStartInfo">The <see cref="ProcessStartInfo"/> for the 'dotnet' process.</param>
    /// <returns></returns>
    /// <exception cref="Exception">An error occurred while running the 'dotnet' process.</exception>
    private static async Task RunDotnetProcessAsync(ProcessStartInfo processStartInfo)
    {
        using Process dotnetProcess = Process.Start(processStartInfo) ?? throw new Exception("Failed to start 'dotnet' process.");

        await dotnetProcess.WaitForExitAsync();

        if (dotnetProcess.ExitCode != 0)
        {
            string dotnetErrorText = await dotnetProcess.StandardError.ReadToEndAsync();

            throw new Exception(dotnetErrorText);
        }
    }

    /// <summary>
    /// Initialize a new .NET solution.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new solution.</param>
    /// <param name="solutionName">The name of the new solution.</param>
    /// <returns></returns>
    public static async Task InitializeDotnetSolutionAsync(string outputDirectory, string solutionName)
    {
        ConsoleUtils.WriteInfo($"\n📦 Initializing .NET solution '{solutionName}.sln'... ", false);

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

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to initialize .NET solution '{solutionName}.sln':\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

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

        ConsoleUtils.WriteInfo($"\n📦 Initializing .NET tool manifest... ", false);

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

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to initialize .NET tool manifest:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

    /// <summary>
    /// Add a .NET tool to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new tool manifest.</param>
    /// <param name="toolName">The name of the tool to add.</param>
    /// <returns></returns>
    public static async Task AddDotnetToolAsync(string outputDirectory, string toolName)
    {
        await InitializeDotnetToolManifestAsync(outputDirectory);

        ConsoleUtils.WriteInfo($"\n📦 Adding .NET tool '{toolName}'... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "tool",
                "install",
                toolName,
                "--tool-manifest",
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception($"Failed to start 'dotnet tool install {toolName}' process.");

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add .NET tool '{toolName}':\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

    /// <summary>
    /// Add the .NET 'gitignore' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new .gitignore file.</param>
    /// <returns></returns>
    public static async Task AddGitIgnoreAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"\n📄 Adding '.gitignore' to project root... ", false);

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

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add .gitignore:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

    /// <summary>
    /// Add the .NET 'build.props' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new build.props file.</param>
    /// <returns></returns>
    public static async Task AddBuildPropsAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"\n📄 Adding 'build.props' to project root... ", false);

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

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add build.props:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

    /// <summary>
    /// Add the .NET 'global.json' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new global.json file.</param>
    /// <returns></returns>
    public static async Task AddGlobalJsonAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"\n📄 Adding 'global.json' to project root... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "globaljson",
                "--roll-forward",
                "latestMinor"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new globaljson' process.");

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add global.json:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

    /// <summary>
    /// Add the .NET 'NuGet.Config' template to the project.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new NuGet.Config file.</param>
    /// <returns></returns>
    public static async Task AddNugetConfigAsync(string outputDirectory)
    {
        ConsoleUtils.WriteInfo($"\n📄 Adding 'NuGet.Config' to project root... ", false);

        ProcessStartInfo dotnetNewProcessStartInfo = CreateDotnetProcessStartInfo(
            arguments: [
                "new",
                "nugetconfig"
            ],
            workingDirectory: outputDirectory
        );

        try
        {
            using Process dotnetNewProcess = Process.Start(dotnetNewProcessStartInfo) ?? throw new Exception("Failed to start 'dotnet new nugetconfig' process.");

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add NuGet.Config:\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }

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

        ConsoleUtils.WriteInfo($"\n📄 Adding project '{projectPathRelative}' to solution '{solutionFilePathRelative}'... ", false);

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

            await dotnetNewProcess.WaitForExitAsync();

            if (dotnetNewProcess.ExitCode != 0)
            {
                string dotnetNewErrorText = await dotnetNewProcess.StandardError.ReadToEndAsync();

                throw new Exception($"Failed to add project '{projectPathRelative}' to solution '{solutionFilePathRelative}':\n\n{dotnetNewErrorText}");
            }
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }
}