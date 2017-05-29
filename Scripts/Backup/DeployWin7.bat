@echo off
net use \\10.78.4.209\Bamboo-Deploy Password1 /user:.\admin
robocopy "C:\Bamboo-Home\InstallShield Source and Setup\LMServer" "\\10.78.4.209\Bamboo-Deploy" *.* /E
IF %ERRORLEVEL% LEQ 3 (exit /b 0) ELSE (exit /b 1)
