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
    /// Updates the C# language server option in the settings.json file.
    /// </summary>
    /// <param name="workspacePath">The path to the workspace directory.</param>
    /// <param name="lspOption">The C# language server option to use.</param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException">The workspace directory does not exist.</exception>
    /// <exception cref="FileNotFoundException">The settings.json file does not exist.</exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task UpdateCsharpLspAsync(string workspacePath, CsharpLspOption lspOption)
    {
        string workspacePathFull = Path.GetFullPath(workspacePath);

        if (!Directory.Exists(workspacePathFull))
        {
            throw new DirectoryNotFoundException("The workspace directory does not exist.");
        }

        string settingsJsonPath = Path.Combine(workspacePathFull, ".vscode/settings.json");

        if (!File.Exists(settingsJsonPath))
        {
            throw new FileNotFoundException("The tasks.json file does not exist.", settingsJsonPath);
        }

        ConsoleUtils.WriteInfo($"- 📄 Updating C# LSP option in tasks.json... ", false);

        try
        {
            using StreamReader reader = new(settingsJsonPath, Encoding.UTF8);

            string settingsJsonContent = await File.ReadAllTextAsync(settingsJsonPath);

            JsonNode? settingsJsonNode = JsonNode.Parse(
                utf8Json: reader.BaseStream,
                documentOptions: new()
                {
                    CommentHandling = JsonCommentHandling.Skip
                }
            );

            reader.Close();

            if (settingsJsonNode is null)
            {
                throw new InvalidOperationException("The settings property is missing.");
            }

            settingsJsonNode
                .UpdateSettingsProperty<bool>("dotnet.server.useOmnisharp", lspOption == CsharpLspOption.OmniSharp, CoreJsonContext.Default.Boolean)
                .UpdateSettingsProperty<string>("dotnet.server.path", lspOption == CsharpLspOption.OmniSharp ? "latest" : "", CoreJsonContext.Default.String);

            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = JsonTypeInfoResolver.Combine(CoreJsonContext.Default)
            };

            string newSettingsJsonContent = settingsJsonNode.ToJsonString(jsonSerializerOptions);

            using StreamWriter writer = new(settingsJsonPath, false, Encoding.UTF8);

            await writer.WriteAsync(newSettingsJsonContent);

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