using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public static partial class TemplatesOperations
{
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

        ConsoleUtils.WriteInfo($"\n📄 Copying 'GitVersion.yml' to project root... ", false);

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
            ConsoleUtils.WriteError("Failed. ❌", false);
            throw;
        }

        ConsoleUtils.WriteSuccess("Done. ✅", false);
    }
}