using System;
using System.IO;
using System.Text;
using System.Threading;
using SharpScss;

namespace ScssMonitorAndCompile
{
    public class Program
    {
        /// <summary>
        /// Input file.
        /// </summary>
        private static string InputFile { get; set; }

        /// <summary>
        /// Output file.
        /// </summary>
        private static string OutputFile { get; set; }

        /// <summary>
        /// The file system watcher.
        /// </summary>
        private static FileSystemWatcher Watcher { get; set; }

        /// <summary>
        /// Whether or not to recompile on next iteration.
        /// </summary>
        private static bool Recompile { get; set; }

        /// <summary>
        /// Init all the things..
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        private static void Main(string[] args)
        {
            // Setup the app.
            ConsoleEx.Init(args);

            // Get input file.
            InputFile = ConsoleEx.GetParamValue("i");

            if (InputFile == null)
            {
                ShowHelp();
                return;
            }

            if (!File.Exists(InputFile))
            {
                ShowError($"Input file not found: {InputFile}");
                return;
            }

            // Get output file.
            OutputFile = ConsoleEx.GetParamValue("o") ??
                         InputFile.Replace(".scss", ".css");

            // Setup monitoring.
            if (!SetupFileSystemWatcher())
            {
                return;
            }

            // Loop til aborted.
            Console.WriteLine("SCSS Monitor and Compile");
            Console.WriteLine();
            Console.WriteLine($"Input file: {InputFile}");
            Console.WriteLine($"Output file: {OutputFile}");
            Console.WriteLine();
            Console.WriteLine("Press CTRL+C to abort.");
            Console.WriteLine();

            while (true)
            {
                if (!Recompile)
                {
                    Thread.Sleep(500);
                    continue;
                }

                Thread.Sleep(500);

                Console.WriteLine($"[{DateTimeOffset.Now}] Detected change. Recompiling SCSS.");
                Recompile = false;

                // Compile SCSS into CSS.
                CompileCss();
            }
        }

        /// <summary>
        /// Compile SCSS into CSS.
        /// </summary>
        private static void CompileCss()
        {
            try
            {
                var scss = File.ReadAllText(
                    InputFile,
                    Encoding.UTF8);

                var result = Scss.ConvertToCss(scss);

                File.WriteAllText(
                    OutputFile,
                    result.Css,
                    Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Setup the FileSystemWatcher that monitors for changes to the source file.
        /// </summary>
        private static bool SetupFileSystemWatcher()
        {
            var path = Path.GetDirectoryName(InputFile);

            if (path == null || !Directory.Exists(path))
            {
                ShowError($"Input file path not found: {path}");
                return false;
            }

            Watcher = new FileSystemWatcher
            {
                Path = path,
                IncludeSubdirectories = true,
                Filter = "*.*"
            };

            Watcher.Changed += WatcherOnChanged;
            Watcher.Created += WatcherOnChanged;
            Watcher.Deleted += WatcherOnChanged;

            Watcher.EnableRaisingEvents = true;

            return true;
        }

        /// <summary>
        /// Show an error.
        /// </summary>
        /// <param name="message">Message to show.</param>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[ERROR] ");

            Console.ResetColor();
            Console.WriteLine(message);
        }

        /// <summary>
        /// Show an error.
        /// </summary>
        /// <param name="ex">Exception to get message from.</param>
        private static void ShowError(Exception ex)
        {
            ShowError(ex.Message);
        }

        /// <summary>
        /// Show the help screen.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("SCSS Monitor and Compile");
            Console.WriteLine("This app will monitor the input file and compile it for each change detected.");
            Console.WriteLine();
            Console.WriteLine("Usage: smc --i <input-scss-file> [--o <output-css-file>]");
            Console.WriteLine();
            Console.WriteLine("  -i  .scss file to monitor.");
            Console.WriteLine("  -o  .css file to output too. If not set this will be the same as input file with a .css ext.");
            Console.WriteLine();
        }

        /// <summary>
        /// Every change to the .scss file triggers.
        /// </summary>
        private static void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == OutputFile)
            {
                return;
            }

            Recompile = true;
        }
    }
}