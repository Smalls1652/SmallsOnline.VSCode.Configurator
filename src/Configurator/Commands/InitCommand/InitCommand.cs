using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands;

/// <summary>
/// Command for initializing a new project.
/// </summary>
public class InitCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InitCommand"/> class.
    /// </summary>
    public InitCommand() : base("init")
    {
        Description = "Initialize a new project.";

        Add(new InitCsharpCommand());
    }
}