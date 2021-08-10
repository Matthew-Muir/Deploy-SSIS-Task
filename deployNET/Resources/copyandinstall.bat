@echo off
copy "C:\install\SFTPUI.dll" "C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Tasks\" /y
copy "C:\install\SecureFTP.dll" "C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Tasks\" /y
copy "C:\install\SFTPUI.dll" "C:\Program Files\Microsoft SQL Server\150\DTS\Tasks\" /y
copy "C:\install\SecureFTP.dll" "C:\Program Files\Microsoft SQL Server\150\DTS\Tasks\" /y

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe" -if "C:\install\SFTPUI.dll"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe" -if "C:\install\SecureFTP.dll"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe" -if "C:\install\WinSCPnet.dll"


pause