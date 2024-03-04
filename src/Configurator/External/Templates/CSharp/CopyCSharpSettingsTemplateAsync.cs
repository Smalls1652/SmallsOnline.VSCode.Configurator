using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class TemplatesOperations
{
    /// <summary>
    /// Copy and modify the C# 'settings.json' template to the '.vscode' directory.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new project.</param>
    /// <param name="solutionName">The name of the new solution.</param>
    /// <returns></returns>
    public static async Task CopyCSharpSettingsTemplateAsync(string outputDirectory, string solutionName)
    {
        EnsureVSCodeDirectoryExists(outputDirectory);

        string templateFilesPath = Path.Combine(
            _coreTemplatesPath,
            "Templates",
            "csharp",
            "VSCode"
        );

        string settingsTemplateJsonPath = Path.Combine(
            templateFilesPath,
            "settings.json"
        );

        string settingsJsonOutputPath = Path.Combine(
            outputDirectory,
            ".vscode",
            "settings.json"
        );

        ConsoleUtils.WriteInfo($"- 📄 Copying 'settings.json' to '.vscode' directory... ", false);

        if (File.Exists(settingsJsonOutputPath))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
            {
                File.Delete(settingsJsonOutputPath);
            }
            else
            {
                ConsoleUtils.WriteWarning("Already exists. 🟠\n", false);
                return;
            }
        }

        try
        {
            File.Copy(
                sourceFileName: settingsTemplateJsonPath,
                destFileName: settingsJsonOutputPath,
                overwrite: true
            );
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        string settingsJsonContent = await File.ReadAllTextAsync(settingsJsonOutputPath);

        settingsJsonContent = settingsJsonContent.Replace(
            oldValue: "{{solutionName}}",
            newValue: $"{solutionName}.sln"
        );

        await File.WriteAllTextAsync(settingsJsonOutputPath, settingsJsonContent);

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}