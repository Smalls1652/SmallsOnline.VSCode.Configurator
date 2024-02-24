using System.CommandLine;
using SmallsOnline.VSCode.Configurator.Commands;

namespace SmallsOnline.VSCode.Configurator;

public class RootCommand : CliRootCommand
{
    public RootCommand() : base("VSCode Configurator")
    {
        Add(new InitCommand());
    }
}