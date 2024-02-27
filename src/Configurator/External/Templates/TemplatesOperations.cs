using SmallsOnline.VSCode.Configurator.Utilities;

namespace SmallsOnline.VSCode.Configurator.External;

/// <summary>
/// A collection of methods for working with Visual Studio Code templates.
/// </summary>
public static partial class TemplatesOperations
{
    /// <summary>
    /// The path to the core templates directory.
    /// </summary>
    private static readonly string _coreTemplatesPath = AppContext.BaseDirectory;

    /// <summary>
    /// Ensure the '.vscode' directory exists in the output directory.
    /// </summary>
    /// <param name="outputDirectory">The output directory for the new project.</param>
    public static void EnsureVSCodeDirectoryExists(string outputDirectory)
    {
        string vscodeDirectoryPath = Path.Combine(
            outputDirectory,
            ".vscode"
        );

        if (!Directory.Exists(vscodeDirectoryPath))
        {
            ConsoleUtils.WriteInfo("- 📁 Creating '.vscode' directory... ", false);
            Directory.CreateDirectory(vscodeDirectoryPath);
            ConsoleUtils.WriteSuccess("Done. ✅\n", false);
        }
    }
}