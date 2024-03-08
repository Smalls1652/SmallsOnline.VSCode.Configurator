using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class VSCodeOperations
{
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