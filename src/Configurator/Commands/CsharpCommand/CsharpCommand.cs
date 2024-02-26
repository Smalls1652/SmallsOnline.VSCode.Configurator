using System.CommandLine;
using SmallsOnline.VSCode.Configurator.Commands.CSharp;

namespace SmallsOnline.VSCode.Configurator.Commands;

/// <summary>
/// Commands for creating and managing C# projects.
/// </summary>
public class CSharpCommand : CliCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpCommand"/> class.
    /// </summary>
    public CSharpCommand() : base("csharp")
    {
        Description = "Commands for creating and managing C# projects.";

        Add(new CSharpInitCommand());
        Add(new CSharpAddProjectCommand());
    }
}