using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands;

public class InitCommand : CliCommand
{
    public InitCommand() : base("init")
    {
        Description = "Initialize a new project.";

        Add(new InitCsharpCommand());
    }
}