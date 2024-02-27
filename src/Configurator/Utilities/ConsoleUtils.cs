namespace SmallsOnline.VSCode.Configurator.Utilities;

/// <summary>
/// Utilities for writing to the console.
/// </summary>
public static class ConsoleUtils
{
    /// <summary>
    /// Writes a message to the console.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="newLine">Whether to write a new line after the message.</param>
    public static void WriteOutput(string message, bool newLine)
    {
        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }
    }

    /// <summary>
    /// Writes a message to the console.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public static void WriteOutput(string message) => WriteOutput(message, true);

    /// <summary>
    /// Writes an error message to the console.
    /// </summary>
    /// <param name="message">The error message to write.</param>
    /// <param name="newLine">Whether to write a new line after the message.</param>
    public static void WriteError(string message, bool newLine)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (newLine)
        {
            Console.Error.WriteLine(message);
        }
        else
        {
            Console.Error.Write(message);
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Writes an error message to the console.
    /// </summary>
    /// <param name="message">The error message to write.</param>
    public static void WriteError(string message) => WriteError(message, true);

    /// <summary>
    /// Writes a warning message to the console.
    /// </summary>
    /// <param name="message">The warning message to write.</param>
    /// <param name="newLine">Whether to write a new line after the message.</param>
    public static void WriteWarning(string message, bool newLine)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Writes a warning message to the console.
    /// </summary>
    /// <param name="message">The warning message to write.</param>
    public static void WriteWarning(string message) => WriteWarning(message, true);

    /// <summary>
    /// Writes an informational message to the console.
    /// </summary>
    /// <param name="message">The informational message to write.</param>
    /// <param name="newLine">Whether to write a new line after the message.</param>
    public static void WriteInfo(string message, bool newLine)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Writes an informational message to the console.
    /// </summary>
    /// <param name="message">The informational message to write.</param>
    public static void WriteInfo(string message) => WriteInfo(message, true);

    /// <summary>
    /// Writes a success message to the console.
    /// </summary>
    /// <param name="message">The success message to write.</param>
    /// <param name="newLine">Whether to write a new line after the message.</param>
    public static void WriteSuccess(string message, bool newLine)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        if (newLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Writes a success message to the console.
    /// </summary>
    /// <param name="message">The success message to write.</param>
    public static void WriteSuccess(string message) => WriteSuccess(message, true);

    /// <summary>
    /// Writes a progress indicator to the console while a task is running.
    /// </summary>
    /// <param name="task">The task to wait for.</param>
    /// <param name="consolePos">The current position of the console cursor.</param>
    /// <returns></returns>
    public static async Task WriteProgressIndicatorAsync(Task task, (int Left, int Top) consolePos)
    {
        Console.CursorVisible = false;
        int counter = 1;
        while (!task.IsCompleted)
        {
            await Task.Delay(100);

            if (counter > 5)
            {
                counter = 1;
            }

            Console.SetCursorPosition(consolePos.Left, consolePos.Top);

            string progressText = counter switch
            {
                2 => "/",
                3 => "-",
                4 => "\\",
                _ => "|"
            };

            Console.Write(progressText);

            counter++;
        }

        Console.SetCursorPosition(consolePos.Left, consolePos.Top);
        Console.CursorVisible = true;
    }
}