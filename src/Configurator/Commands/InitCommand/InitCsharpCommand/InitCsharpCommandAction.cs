using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using SmallsOnline.VSCode.Configurator.External;
using SmallsOnline.VSCode.Configurator.Models.Commands;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.Commands;

public class InitCsharpCommandAction : AsynchronousCliAction
{
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
    
    private static string ParseSolutionNameArgument(ParseResult parseResult)
    {
        string? solutionName = parseResult.GetValue<string>("--solution-name");

        if (solutionName is null || string.IsNullOrEmpty(solutionName) || string.IsNullOrWhiteSpace(solutionName))
        {
            throw new NullReferenceException("The solution name is required.");
        }

        return solutionName;
    }

    private static string ParseOutputDirectoryArgument(ParseResult parseResult)
    {
        string? outputDirectory = parseResult.GetValue<string>("--output-directory");

        if (outputDirectory is null || string.IsNullOrEmpty(outputDirectory) || string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new NullReferenceException("The output directory is required.");
        }

        return outputDirectory;
    }
}