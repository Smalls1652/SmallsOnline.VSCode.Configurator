using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

/// <summary>
/// A collection of methods for interacting with Visual Studio Code configuration files.
/// </summary>
public static class VSCodeOperations
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
        ConsoleUtils.WriteInfo($"👉 Adding project to tasks.json... ", false);

        string? rootProjectDirectoryPath = Path.GetDirectoryName(solutionPath);

        if (rootProjectDirectoryPath is null)
        {
            throw new InvalidOperationException("The solution file path is invalid.");
        }

        string projectPathRelative = Path.GetRelativePath(rootProjectDirectoryPath, projectPath);

        string vscodeDirectoryPath = Path.Combine(rootProjectDirectoryPath, ".vscode");
        string tasksJsonPath = Path.Combine(vscodeDirectoryPath, "tasks.json");

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

    /// <summary>
    /// Gets an input node, in the 'inputs' property, based off of the 'id' property.
    /// </summary>
    /// <param name="inputsNode">The <see cref="JsonNode"/> representing the 'inputs' property.</param>
    /// <param name="id">The value of the 'id' property to search for.</param>
    /// <returns>The <see cref="JsonNode"/> representing the input.</returns>
    /// <exception cref="InvalidOperationException">An issue occurred while searching for the input.</exception>
    private static JsonNode GetInputNodeById(this JsonNode? inputsNode, string id)
    {
        if (inputsNode is null)
        {
            throw new InvalidOperationException("The inputs property is missing.");
        }

        if (inputsNode.GetValueKind() != JsonValueKind.Array)
        {
            throw new InvalidOperationException("The inputs property is not an array.");
        }

        try
        {
            foreach (var inputElement in inputsNode.AsArray())
            {
                JsonNode? inputIdProperty = inputElement!["id"];

                if (inputIdProperty is null)
                {
                    continue;
                }

                if (inputIdProperty.GetValueKind() != JsonValueKind.String)
                {
                    continue;
                }

                if (inputIdProperty.GetValue<string>() == id)
                {
                    return inputElement;
                }
            }

            throw new InvalidOperationException("The projectItem input is missing.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Adds an option to an input node in the 'inputs' property.
    /// </summary>
    /// <param name="inputsNode">The <see cref="JsonNode"/> representing the 'inputs' property.</param>
    /// <param name="inputId">The value of the 'id' property to add the new option to.</param>
    /// <param name="label">The label for the new option.</param>
    /// <param name="value">The value for the new option.</param>
    /// <returns>The modified <see cref="JsonNode"/> representing the 'inputs' property.</returns>
    /// <exception cref="InvalidOperationException">An issue occurred while adding the option to the input node.</exception>
    private static JsonNode AddOptionToInputNode(this JsonNode inputsNode, string inputId, string label, string value)
    {
        if (inputsNode is null)
        {
            throw new InvalidOperationException("The inputs property is missing.");
        }

        JsonNode inputNode = inputsNode.GetInputNodeById(inputId);

        int inputNodeIndex = inputNode.GetElementIndex();

        JsonNode? optionsNode = inputNode["options"];

        if (optionsNode is null)
        {
            throw new InvalidOperationException("The options property is missing.");
        }

        if (optionsNode.GetValueKind() != JsonValueKind.Array)
        {
            throw new InvalidOperationException("The options property is not an array.");
        }

        JsonArray optionsArray = optionsNode.AsArray();

        JsonNode[] newOptionsList = new JsonNode[optionsArray.Count + 1];
        int lastIndex = 0;

        if (optionsArray.Count != 0)
        {
            for (int i = 0; i < optionsArray.Count; i++)
            {
                JsonNode? valueProperty = optionsArray[i]!["value"];

                if (valueProperty is null)
                {
                    continue;
                }

                if (valueProperty.GetValueKind() != JsonValueKind.String)
                {
                    continue;
                }

                if (valueProperty.GetValue<string>() == value)
                {
                    throw new InvalidOperationException("The item already exists in the tasks.json file.");
                }

                newOptionsList[i] = new JsonObject(
                    properties: [
                        new KeyValuePair<string, JsonNode?>("label", JsonValue.Create<string>(optionsArray[i]!["label"]!.GetValue<string>(), CoreJsonContext.Default.String)),
                    new KeyValuePair<string, JsonNode?>("value", JsonValue.Create<string>(optionsArray[i]!["value"]!.GetValue<string>(), CoreJsonContext.Default.String))
                    ]
                );

                lastIndex = i + 1;
            }
        }

        JsonObject newOption = new(
            properties: [
                new KeyValuePair<string, JsonNode?>("label", JsonValue.Create<string>(label, CoreJsonContext.Default.String)),
                new KeyValuePair<string, JsonNode?>("value", JsonValue.Create<string>(value, CoreJsonContext.Default.String))
            ]
        );

        if (optionsArray.Count == 0)
        {
            inputNode["default"] = JsonValue.Create<string>(value, CoreJsonContext.Default.String);
        }

        newOptionsList[lastIndex] = newOption;

        JsonArray optionsReplacement = new(
            items: newOptionsList
        );

        inputNode["options"] = optionsReplacement;

        inputsNode[inputNodeIndex] = inputNode.DeepClone();

        return inputsNode;
    }
}