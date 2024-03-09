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
    /// <summary>
    /// Update the value of a property in the settings.json file.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="settingsNode">The settings.json node.</param>
    /// <param name="propertyName">The name of the property to update.</param>
    /// <param name="propertyValue">The value to update the property with.</param>
    /// <param name="jsonTypeInfo">The JSON type information for the property value.</param>
    /// <returns>The updated settings.json node.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static JsonObject UpdateSettingsProperty<T>(this JsonNode settingsNode, string propertyName, T propertyValue, JsonTypeInfo<T> jsonTypeInfo)
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

        return settingsObject;
    }
}