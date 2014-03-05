using System.Management.Automation;
using System.Runtime.InteropServices;
using System.IO;

namespace PsIniFile
{
    // http://stackoverflow.com/questions/13897918/how-can-i-get-the-current-directory-in-powershell-cmdlet
    // http://www.codeproject.com/Articles/1990/INI-Class-using-C

    [Cmdlet(VerbsCommon.Set, "IniFile")]
    public class SetIniFile: PSCmdlet
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [Parameter(Mandatory = true)]
        public string Section { get; set; }

        [Parameter(Mandatory = true)]
        public string Key { get; set; }

        [Parameter(Mandatory = true)]
        public string Value { get; set; }

        [Parameter(Mandatory = true)]
        public string IniFile { get; set; }

        protected override void EndProcessing()
        {
            // WritePrivateProfileString expects IniFile to be fully-qualified,
            // otherwise it will search for it on the Windows search path.
            // If the IniFile exists in the current directory, then that's the
            // one the user probably wants, so 
            var fileExists = File.Exists(IniFile);
            var pathRooted = Path.IsPathRooted(IniFile);

            WriteDebug("fileExists:" + fileExists + " pathRooted:" + pathRooted);

            if (fileExists && !pathRooted)
            {
                var cwd = SessionState.Path.CurrentFileSystemLocation;
                IniFile = Path.Combine(cwd.Path, IniFile);
                WriteDebug("Expanded IniFile to " + IniFile);
            }

            var returnCode = WritePrivateProfileString(Section, Key, Value, IniFile);
            WriteDebug("WritePrivateProfileString return code: " + returnCode);
        }
    }
}
