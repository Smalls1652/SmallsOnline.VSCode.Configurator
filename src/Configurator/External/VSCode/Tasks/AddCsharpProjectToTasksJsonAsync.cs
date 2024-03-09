using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class VSCodeOperations
{
    /// <summary>
    /// Adds a C# project to the tasks.json file.
    /// </summary>
    /// <param name="solutionPath">The path to the solution file.</param>
    /// <param name="projectPath">The path to the project.</param>
    /// <param name="projectFriendlyName">A friendly name for the project.</param>
    /// <param name="isRunnable">Whether the project is runnable with 'dotnet run'.</param>
    /// <param name="isWatchable">Whether the project is watchable with 'dotnet watch'.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">The solution file path is invalid.</exception>
    /// <exception cref="DirectoryNotFoundException">The .vscode directory does not exist.</exception>
    /// <exception cref="FileNotFoundException">The tasks.json file does not exist.</exception>
    public static async Task AddCsharpProjectToTasksJson(string solutionPath, string projectPath, string projectFriendlyName, bool isRunnable, bool isWatchable)
    {
        string? rootProjectDirectoryPath = Path.GetDirectoryName(solutionPath);

        if (rootProjectDirectoryPath is null)
        {
            throw new InvalidOperationException("The solution file path is invalid.");
        }

        string projectPathRelative = Path.GetRelativePath(rootProjectDirectoryPath, projectPath);

        string vscodeDirectoryPath = Path.Combine(rootProjectDirectoryPath, ".vscode");
        string tasksJsonPath = Path.Combine(vscodeDirectoryPath, "tasks.json");

        ConsoleUtils.WriteInfo($"- 📄 Adding '{projectPathRelative}' to tasks.json... ", false);

        if (!Directory.Exists(vscodeDirectoryPath))
        {
            throw new DirectoryNotFoundException("The .vscode directory does not exist.");
        }

        if (!File.Exists(tasksJsonPath))
        {
            throw new FileNotFoundException("The tasks.json file does not exist.", tasksJsonPath);
        }

        using StreamReader reader = new(tasksJsonPath, Encoding.UTF8);

        string tasksJsonContent = await File.ReadAllTextAsync(tasksJsonPath);

        JsonNode? tasksJsonNode = JsonNode.Parse(
            utf8Json: reader.BaseStream,
            documentOptions: new()
            {
                CommentHandling = JsonCommentHandling.Skip
            }
        );

        reader.Close();

        if (tasksJsonNode is null)
        {
            throw new InvalidOperationException("The tasks.json file is invalid.");
        }

        try
        {
            JsonNode? inputsNode = tasksJsonNode["inputs"]!
                .AddOptionToInputNode(
                    inputId: "projectItem",
                    label: projectFriendlyName,
                    value: projectPathRelative
                );

            if (isRunnable)
            {
                inputsNode.AddOptionToInputNode(
                    inputId: "runProject",
                    label: projectFriendlyName,
                    value: projectPathRelative
                );
            }

            if (isWatchable)
            {
                inputsNode.AddOptionToInputNode(
                    inputId: "watchProject",
                    label: projectFriendlyName,
                    value: projectPathRelative
                );
            }

            tasksJsonNode["inputs"] = inputsNode;

            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = JsonTypeInfoResolver.Combine(CoreJsonContext.Default)
            };

            string newTasksJsonContent = tasksJsonNode.ToJsonString(jsonSerializerOptions);

            using StreamWriter writer = new(tasksJsonPath, false, Encoding.UTF8);

            await writer.WriteAsync(newTasksJsonContent);

            writer.Close();
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}