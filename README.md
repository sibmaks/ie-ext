# InternetExplorerExtension
Addon for IE 11 - Created with tutorial here: https://stackoverflow.com/questions/13493596/internet-explorer-extensions

In the project properties, do the following:

- Sign the assembly with a strong-key;
- In the Debug tab, set Start External Program to C:\Program Files (x86)\Internet Explorer\iexplore.exe
- In the Debug tab, set Command Line Arguments to http://msdn.microsoft.com/en-us/library/ms976373.aspx#bho_getintouch
- In the Build Events tab, set Post-build events command line to:

"%ProgramFiles(x86)%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\gacutil.exe" /f /i "$(TargetDir)$(TargetFileName)"

"%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" /unregister "$(TargetDir)$(TargetFileName)"

"%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" "$(TargetDir)$(TargetFileName)"
Attention: even though my computer is x64, I used the path of the non-x64 gacutil.exe and it worked... the one specific for x64 is at:

C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\gacutil.exe
64bit IE Needs 64bit-compiled and 64bit-registered BHO. Though I could only debug using 32bit IE11, the 32bit registered extension also worked by running 64bit IE11.

This answer appears to have some additional info about this: https://stackoverflow.com/a/23004613/195417

If you need to, you can use the 64bit regasm:

%windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe
