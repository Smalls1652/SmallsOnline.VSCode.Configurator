using System.CommandLine;
using System.CommandLine.Invocation;
using SmallsOnline.VSCode.Configurator.External;
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
        CSharpAddProjectCommandOptions options;
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
            ConsoleUtils.WriteInfo("🚀 Add project");
            await DotnetOperations.AddProjectToSolutionAsync(options.SolutionFilePath, options.ProjectPath);
            await VSCodeOperations.AddCsharpProjectToTasksJson(options.SolutionFilePath, options.ProjectPath, options.ProjectFriendlyName, options.IsRunnable, options.IsWatchable);
        }
        catch (DotnetOperationException ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}\n\n{ex.ProcessErrorText}");
            return 1;
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            return 1;
        }

        ConsoleUtils.WriteSuccess($"\n🥳 '{Path.GetRelativePath(Directory.GetCurrentDirectory(), options.ProjectPath)}' has been configured for the workspace.");

        return 0;
    }
}
