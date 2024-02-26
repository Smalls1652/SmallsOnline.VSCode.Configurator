using System.CommandLine;
using SmallsOnline.VSCode.Configurator.Commands;

namespace SmallsOnline.VSCode.Configurator;

/// <summary>
/// The root command for the CLI.
/// </summary>
public class RootCommand : CliRootCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RootCommand"/> class.
    /// </summary>
    public RootCommand() : base("VSCode Configurator")
    {
        Add(new CSharpCommand());
    }
}