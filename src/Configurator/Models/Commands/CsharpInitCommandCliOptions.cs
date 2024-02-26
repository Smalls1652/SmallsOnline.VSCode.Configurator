using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Models.Commands;

/// <summary>
/// Options for the 'csharp init' command.
/// </summary>
public class CsharpInitCommandCliOptions
{
    /// <summary>
    /// Create a new instance of <see cref="CsharpInitCommandCliOptions"/>.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    public CsharpInitCommandCliOptions(ParseResult parseResult)
    {
        OutputDirectory = ParseOutputDirectoryArgument(parseResult);
        SolutionName = ParseSolutionNameArgument(parseResult);
        AddGitVersion = ParseAddGitVersionArgument(parseResult);
        AddNugetConfig = ParseAddNugetConfigArgument(parseResult);
    }

    /// <summary>
    /// The output directory for the new project.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// The name of the new solution.
    /// </summary>
    public string SolutionName { get; set; }

    /// <summary>
    /// Whether to add GitVersion to the new project.
    /// </summary>
    public bool AddGitVersion { get; set; }

    /// <summary>
    /// Whether to add a NuGet.Config file to the new project.
    /// </summary>
    public bool AddNugetConfig { get; set; }

    /// <summary>
    /// Parse the output directory argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>The output directory for the new project.</returns>
    /// <exception cref="NullReferenceException">Thrown when the output directory is not provided.</exception>
    private static string ParseOutputDirectoryArgument(ParseResult parseResult)
    {
        string? outputDirectory = parseResult.GetValue<string>("--output-directory");

        if (outputDirectory is null || string.IsNullOrEmpty(outputDirectory) || string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new NullReferenceException("The output directory is required.");
        }

        outputDirectory = Path.GetFullPath(outputDirectory);

        return outputDirectory;
    }

    /// <summary>
    /// Parse the '--solution-name' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>The name of the new solution.</returns>
    /// <exception cref="NullReferenceException">Thrown when the solution name is not provided.</exception>
    private string ParseSolutionNameArgument(ParseResult parseResult)
    {
        string? solutionName = parseResult.GetValue<string>("--solution-name");

        if (solutionName is null || string.IsNullOrEmpty(solutionName) || string.IsNullOrWhiteSpace(solutionName))
        {
            solutionName = new DirectoryInfo(OutputDirectory).Name;
        }

        return solutionName;
    }

    /// <summary>
    /// Parse the '--add-gitversion' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>Whether to add GitVersion to the new project.</returns>
    private static bool ParseAddGitVersionArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--add-gitversion");
    }

    /// <summary>
    /// Parse the '--add-nuget-config' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>Whether to add a NuGet.Config file to the new project.</returns>
    private static bool ParseAddNugetConfigArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--add-nuget-config");
    }
}