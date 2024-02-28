using System.CommandLine;
using System.CommandLine.Invocation;
using SmallsOnline.VSCode.Configurator.External;
using SmallsOnline.VSCode.Configurator.Models.Commands;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.Commands.CSharp;

/// <summary>
/// Action for the `csharp init` command.
/// </summary>
public class CSharpInitCommandAction : AsynchronousCliAction
{
    /// <inheritdoc />
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        CSharpInitCommandCliOptions options;
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
            ConsoleUtils.WriteInfo("🚀 Basic");
            await DotnetOperations.AddGlobalJsonAsync(options.OutputDirectory);

            ConsoleUtils.WriteInfo("\n🚀 Git");
            await GitOperations.InitializeGitRepositoryAsync(options.OutputDirectory);
            await DotnetOperations.AddGitIgnoreAsync(options.OutputDirectory);

            ConsoleUtils.WriteInfo("\n🚀 .NET");
            await DotnetOperations.InitializeDotnetSolutionAsync(options.OutputDirectory, options.SolutionName);
            await DotnetOperations.AddBuildPropsAsync(options.OutputDirectory);

            if (options.AddNugetConfig)
            {
                await DotnetOperations.AddNugetConfigAsync(options.OutputDirectory);
            }

            if (options.AddGitVersion)
            {
                ConsoleUtils.WriteInfo("\n🚀 GitVersion");
                await DotnetOperations.AddDotnetToolAsync(options.OutputDirectory, "GitVersion.Tool");
                TemplatesOperations.CopyCSharpGitVersionYaml(options.OutputDirectory);
            }
            
            ConsoleUtils.WriteInfo("\n🚀 VSCode configs");
            await TemplatesOperations.CopyCSharpSettingsTemplateAsync(options.OutputDirectory, options.SolutionName);
            await TemplatesOperations.CopyCSharpTasksTemplateAsync(options.OutputDirectory, options.SolutionName);

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

        ConsoleUtils.WriteSuccess("\n🥳 VSCode project initialized!");
        return 0;
    }
}