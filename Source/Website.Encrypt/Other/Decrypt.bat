@echo off
rem *********************************************
rem **** DECRYPT WEB.CONFIG
rem *********************************************

rem Step 1: Move to folder aspnet_iis
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319

rem Step 2: Decrypt web.config
aspnet_regiis -pd "connectionStrings" -site "Portal" -app "/"