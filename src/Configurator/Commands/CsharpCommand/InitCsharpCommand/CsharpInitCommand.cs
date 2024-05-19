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

        Options.Add(
            new CliOption<string>("--output-directory")
            {
                Description = "The output directory for the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => Environment.CurrentDirectory
            }
        );

        Options.Add(
            new CliOption<string>("--solution-name")
            {
                Description = "The default solution file.",
                Required = false
            }
        );

        Options.Add(
            new CliOption<bool>("--add-gitversion")
            {
                Description = "Whether to add GitVersion to the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        Options.Add(
            new CliOption<bool>("--add-nuget-config")
            {
                Description = "Whether to add a NuGet.Config file to the new project.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        Options.Add(
            new CliOption<bool>("--enable-centrally-managed-packages")
            {
                Description = "Whether to enable centrally managed packages.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

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

        Options.Add(lspOption);

        Action = new CSharpInitCommandAction();
    }
}
