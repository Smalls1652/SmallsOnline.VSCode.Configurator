using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using SmallsOnline.VSCode.Configurator.Models.VSCode;
using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class VSCodeOperations
{
    private static JsonNode UpdateSettingsProperty<T>(this JsonNode settingsNode, string propertyName, T propertyValue, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (settingsNode.GetValueKind() != JsonValueKind.Object)
        {
            throw new InvalidOperationException("The settings property is not an object.");
        }

        JsonObject settingsObject = settingsNode.AsObject();

        JsonNode? propertyNode = settingsObject[propertyName];

        if (propertyNode is null)
        {
            settingsObject.Add(
                propertyName: propertyName,
                value: JsonValue.Create<T>(propertyValue, jsonTypeInfo)
            );
        }
        else
        {
            propertyNode = JsonValue.Create<T>(propertyValue, jsonTypeInfo);

            settingsObject[propertyName] = propertyNode;
        }

        settingsNode.ReplaceWith(settingsObject);

        return settingsNode;
    }
}