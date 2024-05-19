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

        Options
            .AddSolutionFilePathOption()
            .AddProjectPathOption()
            .AddProjectFriendlyNameOption()
            .AddIsRunnableOption()
            .AddIsWatchableOption();

        Action = new CSharpAddProjectCommandAction();
    }
}

file static class CSharpAddProjectCommandExtensions
{
    /// <summary>
    /// Add '--solution-file-path' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddSolutionFilePathOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<string>("--solution-file-path")
            {
                Description = "The solution file to add the project to.",
                Required = false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--project-path' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddProjectPathOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<string>("--project-path")
            {
                Description = "The path to the project.",
                Required = true
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--project-friendly-name' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddProjectFriendlyNameOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<string>("--project-friendly-name")
            {
                Description = "The friendly name of the project.",
                Required = false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--is-runnable' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddIsRunnableOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<bool>("--is-runnable")
            {
                Description = "Whether the project is runnable.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        return options;
    }

    /// <summary>
    /// Add '--is-watchable' option to the command.
    /// </summary>
    /// <param name="options">The options to add to.</param>
    /// <returns>The <see cref="IList{T}"/> for chaining.</returns>
    public static IList<CliOption> AddIsWatchableOption(this IList<CliOption> options)
    {
        options.Add(
            new CliOption<bool>("--is-watchable")
            {
                Description = "Whether the project is watchable.",
                Required = false,
                DefaultValueFactory = (defaultValue) => false
            }
        );

        return options;
    }
}
