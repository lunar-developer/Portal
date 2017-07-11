@echo off
rem *********************************************
rem **** ENCRYPT WEB.CONFIG
rem *********************************************

rem Step 1: Move to folder aspnet_iis
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319

rem Step 2: Encrypt web.config
aspnet_regiis -pe "connectionStrings" -site "Portal" -app "/" -prov "ConnectionEncryptionProvider"