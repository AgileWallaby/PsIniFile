using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PsIniFile
{
    [Cmdlet(VerbsCommon.Get, "IniFile")]
    public class GetIniFile: PSCmdlet
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key,string def, StringBuilder retVal, int size,string filePath);

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
            bool fileExists = File.Exists(IniFile);
            bool pathRooted = Path.IsPathRooted(IniFile);

            WriteObject("fileExists:" + fileExists + " pathRooted:" + pathRooted);

            if (fileExists && !pathRooted)
            {
                //var cwd = Directory.GetCurrentDirectory();
                var cwd = this.SessionState.Path.CurrentFileSystemLocation;
                IniFile = Path.Combine(cwd.Path, IniFile);
                WriteObject("Expanded IniFile to " + IniFile);
            }
            var result = WritePrivateProfileString(Section, Key, Value, IniFile);
            WriteObject("The result is " + result, true);
        }
    }
}
