using System.CommandLine;
using System.CommandLine.Completions;

namespace SmallsOnline.VSCode.Configurator.Commands.CSharp;

/// <summary>
/// Command for initializing a new C# project.
/// </summary>
public class CSharpInitCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpInitCommand"/> class.
    /// </summary>
    public CSharpInitCommand() : base("init")
    {
        Description = "Initialize a new C# project.";

        Options
            .AddOutputDirectoryOption()
            .AddSolutionNameOption()
            .AddGitVersionOption()
            .AddNuGetConfigOption()
            .AddCentrallyManagedPackagesOption()
            .AddCSharpLspOption();

        Action = new CSharpInitCommandAction();
    }
}

file static class CSharpInitCommandExtensions
{
    /// <summary>
    /// Add '--output-directory' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns> 
    public static IList<CliOption> AddOutputDirectoryOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<string>("--output-directory")
            {
                Description = "The output directory for the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => Environment.CurrentDirectory
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--solution-name' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddSolutionNameOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<string>("--solution-name")
            {
                Description = "The default solution file.",
                Required = false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--add-gitversion' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddGitVersionOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<bool>("--add-gitversion")
            {
                Description = "Whether to add GitVersion to the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--add-nuget-config' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddNuGetConfigOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<bool>("--add-nuget-config")
            {
                Description = "Whether to add a NuGet.Config file to the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--enable-centrally-managed-packages' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddCentrallyManagedPackagesOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<bool>("--enable-centrally-managed-packages")
            {
                Description = "Whether to enable centrally managed packages.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--csharp-lsp' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddCSharpLspOption(this IList<CliOption> options)
    {
        CliOption<string> lspOption = new("--csharp-lsp")
        {
            Description = "The C# language server to use.",
            Required = false,
            DefaultValueFactory = (defaultValue) => "OmniSharp"
        };

        lspOption.CompletionSources.Add(
            (CompletionContext context) =>
            {
                List<CompletionItem> items = [
                    new("OmniSharp"),
                    new("CsharpLsp")
                ];

                return items;
            }
        );

        options.Add(lspOption);

        return options;
    }
}
