@echo off
rem *********************************************
rem **** ENCRYPT WEB.CONFIG
rem *********************************************

rem Step 1: Get current folder
@setlocal enableextensions
@cd /d "%~dp0"
set currentFolder=%cd%

rem Step 2: Move to folder aspnet_iis
cd "C:\Windows\Microsoft.NET\Framework\v4.0.30319"

rem Step 3: Import RSA key to IIS
@echo "%currentFolder%\ConnectionEncryptionKeys.xml"
aspnet_regiis -pi "ConnectionEncryptionKeys" "%currentFolder%\ConnectionEncryptionKeys.xml"

rem Step 4: Grant permission to access RSA key for User & Application Pool
aspnet_regiis -pa "ConnectionEncryptionKeys" "NT AUTHORITY\NETWORK SERVICE"
aspnet_regiis.exe -pa "ConnectionEncryptionKeys" "IIS APPPOOL\Portal"

rem Step 5: Encrypt Web.config
aspnet_regiis -pe "connectionStrings" -site "Portal" -app "/" -prov "ConnectionEncryptionProvider"

rem Step 6: Remove RSA key
del "%currentFolder%\ConnectionEncryptionKeys.xml"
pause