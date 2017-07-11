@echo off
rem *********************************************
rem **** GENERATE RSA KEY
rem *********************************************

rem Step 1: Move to folder aspnet_iis
cd "C:\Windows\Microsoft.NET\Framework\v4.0.30319"

rem Step 2: Decrypt web.config (If already encrypted)
aspnet_regiis -pd "connectionStrings" -site "Portal" -app "/"

rem Step 3: Remove Permission access to RSA Key (If any)
aspnet_regiis -pr "ConnectionEncryptionKeys" "NT AUTHORITY\NETWORK SERVICE"

rem Step 4: Remove RSA Key from IIS
aspnet_regiis -pz "ConnectionEncryptionKeys"

rem Step 5: Add RSA Key to IIS
aspnet_regiis -pc "ConnectionEncryptionKeys" -exp

rem Step 6: Grant permission to access RSA Key for user "NT AUTHORITY\NETWORK SERVICE"
aspnet_regiis -pa "ConnectionEncryptionKeys" "NT AUTHORITY\NETWORK SERVICE"

rem Step 7: Generate RSA key
@setlocal enableextensions
@cd /d "%~dp0"
set mypath=%cd%
cd "%mypath%"
DeEnCrypt.exe genkey
pause