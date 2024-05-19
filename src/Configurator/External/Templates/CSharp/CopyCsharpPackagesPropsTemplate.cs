using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

public partial class TemplatesOperations
{
    /// <summary>
    /// Copies the 'Directory.Packages.props' file to the project root.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new project.</param>
    public static void CopyCsharpPackagesPropsTemplate(string outputDirectory)
    {string templateFilesPath = Path.Combine(
            _coreTemplatesPath,
            "Templates",
            "csharp",
            "PropsFiles"
        );

        string directoryPackagesPropsTemplatePath = Path.Combine(
            templateFilesPath,
            "Directory.Packages.props"
        );

        string directoryPackagesPropsOutputPath = Path.Combine(
            outputDirectory,
            "Directory.Packages.props"
        );

        ConsoleUtils.WriteInfo($"- 📄 Copying 'Directory.Packages.props' to project root... ", false);

        if (File.Exists(directoryPackagesPropsOutputPath))
        {
            if (ConsoleUtils.PromptToOverwriteFile())
            {
                File.Delete(directoryPackagesPropsOutputPath);
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
                sourceFileName: directoryPackagesPropsTemplatePath,
                destFileName: directoryPackagesPropsOutputPath,
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
