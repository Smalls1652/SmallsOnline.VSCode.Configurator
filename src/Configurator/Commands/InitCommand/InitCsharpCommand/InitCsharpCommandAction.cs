using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.External;
using SmallsOnline.VSCode.Configurator.Models.Commands;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.Commands;

/// <summary>
/// Action for the `init csharp` command.
/// </summary>
public class InitCsharpCommandAction : AsynchronousCliAction
{
    /// <inheritdoc />
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        InitCsharpCommandCliOptions options;
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
            await GitOperations.InitializeGitRepositoryAsync(options.OutputDirectory);

            await DotnetOperations.AddGlobalJsonAsync(options.OutputDirectory);
            await DotnetOperations.AddGitIgnoreAsync(options.OutputDirectory);

            await DotnetOperations.InitializeDotnetSolutionAsync(options.OutputDirectory, options.SolutionName);
            await DotnetOperations.AddBuildPropsAsync(options.OutputDirectory);
            
            await TemplatesOperations.CopyCSharpSettingsTemplateAsync(options.OutputDirectory, options.SolutionName);
            await TemplatesOperations.CopyCSharpTasksTemplateAsync(options.OutputDirectory, options.SolutionName);

            if (options.AddNugetConfig)
            {
                await DotnetOperations.AddNugetConfigAsync(options.OutputDirectory);
            }

            if (options.AddGitVersion)
            {
                await DotnetOperations.AddDotnetToolAsync(options.OutputDirectory, "GitVersion.Tool");
                TemplatesOperations.CopyCSharpGitVersionYaml(options.OutputDirectory);
            }
        }
        catch (Exception ex)
        {
            ConsoleUtils.WriteError($"\n❌ {ex.Message}");
            return 1;
        }

        ConsoleUtils.WriteSuccess("\n\n🥳 VSCode project initialized!");
        return 0;
    }
}