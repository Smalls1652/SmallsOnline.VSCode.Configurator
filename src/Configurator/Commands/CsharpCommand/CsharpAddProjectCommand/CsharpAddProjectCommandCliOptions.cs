using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands.CSharp;

/// <summary>
/// Options for the 'csharp add-project' command.
/// </summary>
public class CSharpAddProjectCommandCliOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpAddProjectCommandCliOptions"/> class.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    public CSharpAddProjectCommandCliOptions(ParseResult parseResult)
    {
        SolutionFilePath = ParseSolutionFilePathArgument(parseResult);
        ProjectPath = ParseProjectPathArgument(parseResult);
        ProjectFriendlyName = ParseProjectFriendlyNameArgument(parseResult);
        IsRunnable = ParseIsRunnableArgument(parseResult);
        IsWatchable = ParseIsWatchableArgument(parseResult);
    }

    /// <summary>
    /// The path to the solution file.
    /// </summary>
    public string SolutionFilePath { get; set; }

    /// <summary>
    /// The path to the project.
    /// </summary>
    public string ProjectPath { get; set; }

    /// <summary>
    /// A friendly name for the project.
    /// </summary>
    public string ProjectFriendlyName { get; set; }

    /// <summary>
    /// Whether the project is runnable with 'dotnet run'.
    /// </summary>
    public bool IsRunnable { get; set; }

    /// <summary>
    /// Whether the project is watchable with 'dotnet watch'.
    /// </summary>
    public bool IsWatchable { get; set; }

    /// <summary>
    /// Parse the '--solution-file-path' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>The path to the solution file.</returns>
    /// <exception cref="NullReferenceException">No solution file path provided.</exception>
    /// <exception cref="InvalidOperationException">Multiple solution files found.</exception>
    /// <exception cref="FileNotFoundException">The solution file does not exist.</exception>
    private static string ParseSolutionFilePathArgument(ParseResult parseResult)
    {
        string? solutionFilePath = parseResult.GetValue<string>("--solution-file-path");

        if (solutionFilePath is null || string.IsNullOrEmpty(solutionFilePath) || string.IsNullOrWhiteSpace(solutionFilePath))
        {
            string[] solutionFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");

            if (solutionFiles.Length == 0)
            {
                throw new NullReferenceException("The solution file path is required.");
            }

            if (solutionFiles.Length > 1)
            {
                throw new InvalidOperationException("Multiple solution files found. Please specify the solution file path.");
            }

            solutionFilePath = solutionFiles[0];
        }

        solutionFilePath = Path.GetFullPath(solutionFilePath);

        if (!File.Exists(solutionFilePath))
        {
            throw new FileNotFoundException("The solution file does not exist.", solutionFilePath);
        }

        return solutionFilePath;
    }

    /// <summary>
    /// Parse the '--project-path' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>The path to the project.</returns>
    /// <exception cref="NullReferenceException">No project path provided.</exception>
    /// <exception cref="DirectoryNotFoundException">The project path does not exist.</exception>
    /// <exception cref="FileNotFoundException">No '.csproj' file exists in the directory specified.</exception>
    private static string ParseProjectPathArgument(ParseResult parseResult)
    {
        string? projectPath = parseResult.GetValue<string>("--project-path");

        if (projectPath is null || string.IsNullOrEmpty(projectPath) || string.IsNullOrWhiteSpace(projectPath))
        {
            throw new NullReferenceException("The project path is required.");
        }

        projectPath = Path.GetFullPath(projectPath);

        if (!Directory.Exists(projectPath))
        {
            throw new DirectoryNotFoundException("The project path does not exist.");
        }

        string[] projectFiles = Directory.GetFiles(projectPath, "*.csproj");

        if (projectFiles.Length == 0)
        {
            throw new FileNotFoundException("No '.csproj' file exists in the directory specified.", projectPath);
        }

        return projectPath;
    }

    /// <summary>
    /// Parse the '--project-friendly-name' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>The friendly name of the project.</returns>
    private string ParseProjectFriendlyNameArgument(ParseResult parseResult)
    {
        string? projectFriendlyName = parseResult.GetValue<string>("--project-friendly-name");

        if (projectFriendlyName is null || string.IsNullOrEmpty(projectFriendlyName) || string.IsNullOrWhiteSpace(projectFriendlyName))
        {
            projectFriendlyName = new DirectoryInfo(ProjectPath).Name;
        }

        return projectFriendlyName;
    }

    /// <summary>
    /// Parse the '--is-runnable' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>Whether the project is runnable.</returns>
    private static bool ParseIsRunnableArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--is-runnable");
    }

    /// <summary>
    /// Parse the '--is-watchable' argument from the command line.
    /// </summary>
    /// <param name="parseResult">The parse result from the command line.</param>
    /// <returns>Whether the project is watchable.</returns>
    private static bool ParseIsWatchableArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--is-watchable");
    }
}
