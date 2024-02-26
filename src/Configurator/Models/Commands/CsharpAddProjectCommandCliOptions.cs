using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Models.Commands;

public class CsharpAddProjectCommandCliOptions
{
    public CsharpAddProjectCommandCliOptions(ParseResult parseResult)
    {
        SolutionFilePath = ParseSolutionFilePathArgument(parseResult);
        ProjectPath = ParseProjectPathArgument(parseResult);
        ProjectFriendlyName = ParseProjectFriendlyNameArgument(parseResult);
        IsRunnable = ParseIsRunnableArgument(parseResult);
        IsWatchable = ParseIsWatchableArgument(parseResult);
    }

    public string SolutionFilePath { get; set; }

    public string ProjectPath { get; set; }

    public string ProjectFriendlyName { get; set; }

    public bool IsRunnable { get; set; }

    public bool IsWatchable { get; set; }

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

    private string ParseProjectFriendlyNameArgument(ParseResult parseResult)
    {
        string? projectFriendlyName = parseResult.GetValue<string>("--project-friendly-name");

        if (projectFriendlyName is null || string.IsNullOrEmpty(projectFriendlyName) || string.IsNullOrWhiteSpace(projectFriendlyName))
        {
            projectFriendlyName = new DirectoryInfo(ProjectPath).Name;
        }

        return projectFriendlyName;
    }

    private bool ParseIsRunnableArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--is-runnable");
    }

    private bool ParseIsWatchableArgument(ParseResult parseResult)
    {
        return parseResult.GetValue<bool>("--is-watchable");
    }
}