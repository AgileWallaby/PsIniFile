using System;
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
            if (returnCode == 0)
            {
                var exception = new InvalidOperationException("Error return code returned by WritePrivateProfileString: " + returnCode);
                ThrowTerminatingError(new ErrorRecord(exception, "WriteProfileProfileStringError", ErrorCategory.InvalidOperation, null));
            }
            WriteVerbose("WritePrivateProfileString return code x: " + returnCode);
        }
    }
}
