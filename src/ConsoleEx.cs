namespace ScssMonitorAndCompile
{
    public class ConsoleEx
    {
        /// <summary>
        /// Command-line arguments.
        /// </summary>
        private static string[] CmdArgs { get; set; }

        /// <summary>
        /// Prefix for each option.
        /// </summary>
        private static string OptionPrefix { get; set; }

        /// <summary>
        /// Setup the app.
        /// </summary>
        /// <param name="cmdArgs">Command-line arguments.</param>
        /// <param name="optionPrefix">Prefix for each option.</param>
        public static void Init(string[] cmdArgs, string optionPrefix = "--")
        {
            CmdArgs = cmdArgs;
            OptionPrefix = optionPrefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetParamValue(string key)
        {
            if (CmdArgs == null ||
                CmdArgs.Length < 2)
            {
                return null;
            }

            for (var i = 0; i < CmdArgs.Length - 1; i++)
            {
                if (CmdArgs[i] == $"{OptionPrefix}{key}")
                {
                    return CmdArgs[i + 1];
                }
            }

            return null;
        }
    }
}