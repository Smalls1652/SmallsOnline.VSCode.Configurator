using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class VSCodeOperations
{
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
}