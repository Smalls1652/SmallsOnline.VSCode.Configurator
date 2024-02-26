using System.CommandLine;

namespace SmallsOnline.VSCode.Configurator.Commands;

public class CsharpCommand : CliCommand
{
    public CsharpCommand() : base("csharp")
    {
        Description = "Commands for creating and managing C# projects.";

        Add(new CsharpAddProjectCommand());
    }
}