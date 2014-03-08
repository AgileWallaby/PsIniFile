using System.Management.Automation;
using System.Runtime.InteropServices;

namespace PsIniFile
{
    [Cmdlet(VerbsCommon.Set, "IniFileValue")]
    public class SetIniFileValue: PSCmdlet
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

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
            IniFile = Utils.ExpandIniFileToFullyQualifiedPath(SessionState, IniFile);
            var returnCode = WritePrivateProfileString(Section, Key, Value, IniFile);
            WriteVerbose("WritePrivateProfileString return code: " + returnCode);
        }
    }
}
