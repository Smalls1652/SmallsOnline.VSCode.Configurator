using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands;

/// <summary>
/// Commands for creating and managing C# projects.
/// </summary>
public class CsharpCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CsharpCommand"/> class.
    /// </summary>
    public CsharpCommand() : base("csharp")
    {
        Description = "Commands for creating and managing C# projects.";

        Add(new CsharpAddProjectCommand());
    }
}