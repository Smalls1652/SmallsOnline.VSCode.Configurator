using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class TemplatesOperations
{
    /// <summary>
    /// Copy and modify the C# 'tasks.json' template to the '.vscode' directory.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new project.</param>
    /// <param name="solutionName">The name of the new solution.</param>
    /// <returns></returns>
    public static async Task CopyCSharpTasksTemplateAsync(string outputDirectory, string solutionName)
    {
        EnsureVSCodeDirectoryExists(outputDirectory);

        string templateFilesPath = Path.Combine(
            _coreTemplatesPath,
            "Templates",
            "csharp",
            "VSCode"
        );
        
        string tasksTemplateJsonPath = Path.Combine(
            templateFilesPath,
            "tasks.json"
        );

        string tasksJsonOutputPath = Path.Combine(
            outputDirectory,
            ".vscode",
            "tasks.json"
        );

        ConsoleUtils.WriteInfo($"- 📄 Copying 'tasks.json' to '.vscode' directory... ", false);

        try
        {
            File.Copy(
                sourceFileName: tasksTemplateJsonPath,
                destFileName: tasksJsonOutputPath,
                overwrite: true
            );
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        string tasksJsonContent = await File.ReadAllTextAsync(tasksJsonOutputPath);

        tasksJsonContent = tasksJsonContent.Replace(
            oldValue: "{{solutionName}}",
            newValue: $"{solutionName}.sln"
        );

        await File.WriteAllTextAsync(tasksJsonOutputPath, tasksJsonContent);

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}