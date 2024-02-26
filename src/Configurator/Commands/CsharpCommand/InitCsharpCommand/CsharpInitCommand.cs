using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands.Csharp;

/// <summary>
/// Command for initializing a new C# project.
/// </summary>
public class CsharpInitCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CsharpInitCommand"/> class.
    /// </summary>
    public CsharpInitCommand() : base("init")
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

        Action = new CsharpInitCommandAction();
    }
}