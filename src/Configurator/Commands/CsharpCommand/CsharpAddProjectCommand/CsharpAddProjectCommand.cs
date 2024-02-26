using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands.CSharp;

/// <summary>
/// Command for adding a new C# project to the workspace.
/// </summary>
public class CSharpAddProjectCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpAddProjectCommand"/> class.
    /// </summary>
    public CSharpAddProjectCommand() : base("add-project")
    {
        Description = "Add a new C# project to the workspace.";

        Options.Add(
            new CliOption<string>("--solution-file-path")
            {
                Description = "The solution file to add the project to.",
                Required = false
            }
        );

        Options.Add(
            new CliOption<string>("--project-path")
            {
                Description = "The path to the project.",
                Required = true
            }
        );

        Options.Add(
            new CliOption<string>("--project-friendly-name")
            {
                Description = "The friendly name of the project.",
                Required = false
            }
        );

        Options.Add(
            new CliOption<bool>("--is-runnable")
            {
                Description = "Whether the project is runnable.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        Options.Add(
            new CliOption<bool>("--is-watchable")
            {
                Description = "Whether the project is watchable.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        Action = new CSharpAddProjectCommandAction();
    }
}