using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
