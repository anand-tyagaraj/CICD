@echo off
net use \\10.78.4.113\Bamboo-Deploy Password1 /user:.\admin
robocopy "C:\Release-Candidate\Product Configuration 1\Release 1\DiskImages\DISK1" "\\10.78.4.113\Bamboo-Deploy" *.* /E
IF %ERRORLEVEL% LEQ 3 (exit /b 0) ELSE (exit /b 1)