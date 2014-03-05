using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Management.Automation;

namespace PsIniFile
{
    [Cmdlet(VerbsCommon.Get, "IniFile")]
    public class GetIniFile: PSCmdlet
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [Parameter(Mandatory = true)]
        public string Section { get; set; }

        [Parameter(Mandatory = true)]
        public string Key { get; set; }

        [Parameter(Mandatory = true)]
        public string IniFile { get; set; }

        protected override void EndProcessing()
        {
            // GetPrivateProfileString expects IniFile to be fully-qualified,
            // otherwise it will search for it on the Windows search path.
            // If the IniFile exists in the current directory, then that's the
            // one the user probably wants, so 
            var pathRooted = Path.IsPathRooted(IniFile);

            if (!pathRooted)
            {
                var cwd = SessionState.Path.CurrentFileSystemLocation;
                var localFile = Path.Combine(cwd.Path, IniFile);
                var fileExists = File.Exists(localFile);

                if (fileExists)
                {
                    IniFile = localFile;
                    WriteDebug("File exists in working directory.");
                }
                else
                {
                    WriteDebug("File does not exist in working directory, GetPrivateProfileString will use Windows search path.");
                }
            }

            var sb = new StringBuilder(255);
            var result = GetPrivateProfileString(Section, Key, "", sb, 255, IniFile);
            WriteDebug("GetPrivateProfileString return code: " + result + " Return Value: " + sb);
            WriteObject(sb.ToString(), false);
        }
    }
}
