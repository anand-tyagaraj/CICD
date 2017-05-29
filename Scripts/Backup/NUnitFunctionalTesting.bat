@echo off
cd "C:\Program Files (x86)\NUnit 2.6.2\bin"
nunit-console "C:\Bamboo-Home\LM Source\LM_CS_CTF4_Theme\Tool_Folder\NUnitTest.dll" /out:c:\Test.txt
pause