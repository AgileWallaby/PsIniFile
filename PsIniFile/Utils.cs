using System.IO;
using System.Management.Automation;

namespace PsIniFile
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// GetPrivateProfileString and WritePrivateProfileString expect the path to the INI file to be fully-qualified,
        /// otherwise it will search for it on the Windows search path.
        /// If the INI file exists in the current directory, then assume that's the one the user wants, to update
        /// the path to be fully-qualified.
        /// </summary>
        /// <param name="sessionState">The state of the cmdlet.</param>
        /// <param name="iniFile">The name of, and possibly relative or absolute path to, the INI file.</param>
        /// <returns></returns>
        internal static string ExpandIniFileToFullyQualifiedPath(SessionState sessionState, string iniFile)
        {
            if (Path.IsPathRooted(iniFile))
            {
                return iniFile;
            }

            // Directory.GetCurrentDirectory() doesn't work in PowerShell cmdlets. Thanks to the following Stack Overflow
            // Q&A for the correct way to do it.
            // http://stackoverflow.com/questions/13897918/how-can-i-get-the-current-directory-in-powershell-cmdlet
            var cwd = sessionState.Path.CurrentFileSystemLocation.Path;

            var localFile = Path.Combine(cwd, iniFile);
            var fileExists = File.Exists(localFile);

            if (fileExists)
            {
                iniFile = localFile;
            }
            return iniFile;
        }
    }
}