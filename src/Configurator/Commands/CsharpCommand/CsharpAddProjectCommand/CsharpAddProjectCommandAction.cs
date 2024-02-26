using System.CommandLine;
using System.CommandLine.Invocation;
using SmallsOnline.VSCode.Configurator.External;
using SmallsOnline.VSCode.Configurator.Models.Commands;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.Commands.Csharp;

/// <summary>
/// Action for the `csharp add-project` command.
/// </summary>
public class CsharpAddProjectCommandAction : AsynchronousCliAction
{
    /// <inheritdoc />
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        CsharpAddProjectCommandCliOptions options;
        try
        {
            options = new(parseResult);
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            return 1;
        }

        ConsoleUtils.WriteInfo($"📄 Solution file path: {options.SolutionFilePath}");
        ConsoleUtils.WriteInfo($"📂 Project path: {options.ProjectPath}");
        ConsoleUtils.WriteInfo($"📝 Project friendly name: {options.ProjectFriendlyName}");

        try
        {
            await DotnetOperations.AddProjectToSolutionAsync(options.SolutionFilePath, options.ProjectPath);
            await VSCodeOperations.AddCsharpProjectToTasksJson(options.SolutionFilePath, options.ProjectPath, options.ProjectFriendlyName, options.IsRunnable, options.IsWatchable);
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            throw;
            //return 1;
        }

        return 0;
    }
}