using System.Text;
using System.Runtime.InteropServices;
using System.Management.Automation;

namespace PsIniFile
{
    [Cmdlet(VerbsCommon.Get, "IniFileValue")]
    public class GetIniFileValue: PSCmdlet
    {
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [Parameter(Mandatory = true)]
        public string Section { get; set; }

        [Parameter(Mandatory = true)]
        public string Key { get; set; }

        [Parameter(Mandatory = true)]
        public string IniFile { get; set; }

        protected override void EndProcessing()
        {
            IniFile = Utils.ExpandIniFileToFullyQualifiedPath(SessionState, IniFile);

            var sb = new StringBuilder(255);
            var returnCode = GetPrivateProfileString(Section, Key, string.Empty, sb, 255, IniFile);
            WriteVerbose("GetPrivateProfileString return code: " + returnCode);
            var result = string.IsNullOrEmpty(sb.ToString()) ? null : sb.ToString();
            WriteObject(result, false);
        }
    }
}
