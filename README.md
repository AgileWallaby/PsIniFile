# PsIniFile #


The 80's called and wants it's file format back...


A PowerShell module for reading and writing .ini files. I thought there would be something like this already in the [PsGet](http://psget.net) directory or [PSCX](http://pscx.codeplex.com) but turns out there isn't, so here it is!

This is basically a wrapper around the [GetPrivateProfileString](http://msdn.microsoft.com/en-us/library/windows/desktop/ms724353(v=vs.85).aspx) and [WritePrivateProfileString](http://msdn.microsoft.com/en-us/library/windows/desktop/ms725501(v=vs.85).aspx) Win32 functions, and contains two cmdlets; [Get-IniFileValue](#get) and [Set-IniFileValue](#set).

### Installation ###

I recommend installing [PsGet](http://psget.net) ('Nuget for PowerShell'), in which case it is as simple as:

    PS C:\> install-module PsIniFile

Otherwise, download a release and unzip to `$home\Documents\WindowsPowerShell\Modules\PsIniFile`


## Get-IniFileValue ##

<a name="get"></a>

Retrieves the value, as a string, of a key within a section in an .ini file.

All of the following parameters are required:

* Section (string): The name of the section containing the key whose value should be returned.
* Key (string): The name of the key containing the value that should be returned.
* IniFile (string): The name of the .ini file to be read. If this is not in the current working directory an absolute or relative path should be used. Otherwise GetPrivateProfileString will search for the file in the Windows directory. 

The Section and Key parameters are case-insensitive.

If the value exists it is returned as a string. Otherwise null is returned for all other cases (missing file, missing section, missing key). 

### Example ###

Given an .ini file named `my.ini` file in the current working directory with the following contents...

    [SectionA]
    KeyA=ValueA
	[SectionB]
    KeyA=ValueB
	KeyB=ValueC

... the following PowerShell session behaviour would be expected:

    PS C:\> Get-IniFileValue -Section SectionA -Key KeyA -IniFile my.ini
    ValueA
	PS C:\> Get-IniFileValue -Section SectionB -Key KeyA -IniFile my.ini
	ValueB
	PS C:\> Get-IniFileValue -Section SectionC -Key KeyA -IniFile my.ini
	PS C:\> # returned null as no SectionC
	PS C:\> Get-IniFileValue -Section SectionA -Key KeyB -IniFile my.ini
	PS C:\> # returned null as no KeyB in SectionA.
	PS C:\> Get-IniFileValue -Section SectionB -Key KeyB -IniFile my.ini
	ValueC  


## Set-IniFileValue ##

<a name="get"></a>

Updates a string value for a key within a section in an .ini file. If the section or key does not already exist, it will be created.

All of the following parameters are required:

* Section (string): The name of the section containing the key whose value should be written.
* Key (string): The name of the key whose value should be written.
* Value (string): The value that should be written.
* IniFile (string): The name of the .ini file to be updated. If this is not in the current working directory an absolute or relative path should be used. Otherwise GetPrivateProfileString will search for the file in the Windows directory; if you are not running as Administrator this will fail. 

## Coming Soon (maybe...) ##

* cmdlets for [GetPrivateProfileSectionNames](http://msdn.microsoft.com/en-us/library/windows/desktop/ms724352(v=vs.85).aspx) and [GetPrivateProfileSection](http://msdn.microsoft.com/en-us/library/windows/desktop/ms724348(v=vs.85).aspx). 