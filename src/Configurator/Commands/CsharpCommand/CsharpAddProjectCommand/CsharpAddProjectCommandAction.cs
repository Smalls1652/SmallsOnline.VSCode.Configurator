using System.CommandLine;
using System.CommandLine.Invocation;
using SmallsOnline.VSCode.Configurator.External;
using SmallsOnline.VSCode.Configurator.Models.Commands;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.Commands.CSharp;

/// <summary>
/// Action for the `csharp add-project` command.
/// </summary>
public class CSharpAddProjectCommandAction : AsynchronousCliAction
{
    /// <inheritdoc />
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        CSharpAddProjectCommandCliOptions options;
        try
        {
            options = new(parseResult);
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            return 1;
        }

        try
        {
            await DotnetOperations.AddProjectToSolutionAsync(options.SolutionFilePath, options.ProjectPath);
            await VSCodeOperations.AddCsharpProjectToTasksJson(options.SolutionFilePath, options.ProjectPath, options.ProjectFriendlyName, options.IsRunnable, options.IsWatchable);
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            return 1;
        }

        return 0;
    }
}