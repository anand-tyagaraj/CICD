@ECHO OFF
:: -----------------------------------------------------------------
:: Description:
:: This batch file is to act as Template for using Klocwork (KW) on project's 
:: codebase and loading issue report on Klocwork central report publishing server.
:: Depending on project's build mechanism, few commands may need change.
::
:: This file is divided into 4 sections.
:: Section 1: Contains configurable project info which may need change (one-time)
:: Section 2: Contains KW tables folder info which may not need change
:: Section 3: Contains Klocwork license and server info which may not need change
:: Section 4: Contains commands to create Klocwork build & load issue report on
::			  Klocwork server. May need change (one-time).
::
:: Pre-requisite: Machine on which this BAT file is executed, should have:
::				- Project's build environment
::				- KW Build Tools installed from KW Server installer
::
:: Troubleshooting:
:: In case of problems while using Klocwork through this batch file,
:: run below command in DOS CMD prompt and send generated KW_LOG.TXT to Helpdesk
:: 
:: KW_1_<SBU_PROJ>.BAT > KW_LOG.TXT
::
:: -----------------------------------------------------------------

:: kwauth.exe --url https://acsklocwork.honeywell.com:443/ --verbose

:: kwadmin --url https://acsklocwork.honeywell.com:443/ create-project HLS_LocationManager --language csharp --copy-sources

:: kwadmin --url https://acsklocwork.honeywell.com:443/ list-projects



:: Section 1:

:: Change it as per your Klocwork project. This should be same as KW project created on KW central server.
:: Set KW project name.
set PROJNAME=HLS_LocationManager
echo PROJNAME: %PROJNAME%

:: Change it as per your project (path of .sln, .vcproj, .csproj, .ewp, .dsp - from where build command can run).
:: This is the folder to update with CM development integration stream.
:: Set project directory path
set PROJDIR=C:\FB LM\Plug_In\PlugIn
echo PROJDIR: %PROJDIR%

:: Change it as per your project drive.
:: Set KW project drive
set PROJDIRDRIVE=C:
echo PROJDIRDRIVE: %PROJDIRDRIVE%

:: Change it as per your project's builder path (path of msbuild.exe, devenv.exe, iarbuild.exe as applicable).
:: Set project's builder path
set BUILDERDIR=C:\Windows\Microsoft.NET\Framework\v4.0.30319
echo BUILDERDIR: %BUILDERDIR%

:: Change it as per your Klocwork installation path.
:: Set KW installation directory path.
set KWROOT=C:\Klocwork\Server 9.6
echo KWROOT: %KWROOT%

:: Change it as per your Klocwork installation drive.
:: Set KW installation drive.
set KWROOTDRIVE=C:
echo KWROOTDRIVE: %KWROOTDRIVE%

:: -----------------------------------------------------------------

:: Section 2:

:: Generally change may NOT be required for this path setting.
:: Set KW tables directory path.
:: Example: If KWROOT is set as 'C:\Klocwork\Server 9.2' in above command then
:: KWTABLES will be set as 'C:\Klocwork\Tables'.
cd %KWROOT%
%KWROOTDRIVE%
cd..
if not exist "%CD%\TABLES" mkdir "TABLES"
set KWTABLES=%CD%\TABLES
echo KWTABLES: %KWTABLES%

:: Generally change may NOT be required for this path setting
:: Example: If KWTABLES is set as 'C:\Klocwork\Tables' and PROJNAME is set as 'SampleProj' in above command then
:: OUTDIR will be set as 'C:\Klocwork\Tables\SampleProj_Tables'.
:: Set KW tables directory path specifically for above set PROJNAME
cd %KWTABLES%
if not exist "%KWTABLES%\%PROJNAME%_TABLES" mkdir "%PROJNAME%_TABLES"
set OUTDIR=%KWTABLES%\%PROJNAME%_TABLES
echo OUTDIR: %OUTDIR%

:: -----------------------------------------------------------------

:: Section 3:

:: Standardized - no change needed unless you have own KW server setup

:: Set KW central report publishing host
set KWHOST=acsklocwork.honeywell.com
echo KWHOST: %KWHOST%

:: Set KW central report publishing host url
set KWHOSTURL=https://acsklocwork.honeywell.com:443
echo KWHOSTURL: %KWHOSTURL%

:: Set KW license host
set LMHOST=acslicenses.honeywell.com
echo LMHOST: %LMHOST%

:: Set KW license timeout environment variable
set FLEXLM_TIMEOUT=10000000
echo FLEXLM_TIMEOUT: %FLEXLM_TIMEOUT%

:: -----------------------------------------------------------------

:: Section 4:

echo "Changing working directory"
cd %PROJDIR%
%PROJDIRDRIVE%

 echo "Creating build specification - %PROJNAME%"
 "%KWROOT%\bin\kwcsprojparser.exe" "%PROJDIR%\Plugin.sln" --list-configs
 "%KWROOT%\bin\kwcsprojparser.exe" -o "%KWTABLES%\%PROJNAME%.out" "%PROJDIR%\Plugin.sln" --config "Debug|Any CPU"

echo "Creating build - %PROJNAME%"
"%KWROOT%\bin\kwbuildproject.exe" --color --verbose "--license-host" "%LMHOST%" "--license-port" "27001" "--url" "%KWHOSTURL%/%PROJNAME%" "%KWTABLES%\%PROJNAME%.out" --tables-directory %OUTDIR% --force

echo "Loading build - %PROJNAME%"
"%KWROOT%\bin\kwadmin.exe" --color --ssl --verbose "--host" "%KWHOST%" "--port" "443"  load %PROJNAME% "%OUTDIR%" --copy-sources


echo "Visit 'http://acsklocwork.honeywell.com' for SCA report."
echo "Use LDAP credentials for login."