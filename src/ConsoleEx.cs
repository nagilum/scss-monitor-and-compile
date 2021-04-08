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
        /// Prefix for each option.
        /// </summary>
        private static string OptionalOptionPrefix { get; set; }

        /// <summary>
        /// Setup the app.
        /// </summary>
        /// <param name="cmdArgs">Command-line arguments.</param>
        /// <param name="optionPrefix">Prefix for each option.</param>
        /// <param name="optionalOptionPrefix">Optional prefix for each option.</param>
        public static void Init(string[] cmdArgs, string optionPrefix = "--", string optionalOptionPrefix = "-")
        {
            CmdArgs = cmdArgs;
            OptionPrefix = optionPrefix;
            OptionalOptionPrefix = optionalOptionPrefix;
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
                if (CmdArgs[i] == key ||
                    CmdArgs[i] == $"{OptionPrefix}{key}" ||
                    CmdArgs[i] == $"{OptionalOptionPrefix}{key}")
                {
                    return CmdArgs[i + 1];
                }
            }

            return null;
        }
    }
}