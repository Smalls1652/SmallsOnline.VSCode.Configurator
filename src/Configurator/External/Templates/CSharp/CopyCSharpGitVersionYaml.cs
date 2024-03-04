using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class TemplatesOperations
{
    /// <summary>
    /// Copies the 'GitVersion.yml' file to the project root.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new project.</param>
    public static void CopyCSharpGitVersionYaml(string outputDirectory)
    {string templateFilesPath = Path.Combine(
            _coreTemplatesPath,
            "Templates",
            "csharp",
            "GitVersion"
        );

        string gitVersionYamlTemplatePath = Path.Combine(
            templateFilesPath,
            "GitVersion.yml"
        );

        string gitVersionYamlOutputPath = Path.Combine(
            outputDirectory,
            "GitVersion.yml"
        );

        ConsoleUtils.WriteInfo($"- 📄 Copying 'GitVersion.yml' to project root... ", false);

        if (File.Exists(gitVersionYamlOutputPath))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
            {
                File.Delete(gitVersionYamlOutputPath);
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
                sourceFileName: gitVersionYamlTemplatePath,
                destFileName: gitVersionYamlOutputPath,
                overwrite: true
            );
        }
        catch (Exception)
        {
            ConsoleUtils.WriteError("Failed. ❌\n", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅\n", false);
    }
}