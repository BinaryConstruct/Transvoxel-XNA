@echo off
setlocal ENABLEDELAYEDEXPANSION

set SOLUTION=TransvoxelXNA

if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" set msbuild="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
if exist "C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" set devenv="C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"
REM for Win64
if exist "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" set devenv="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

echo Building TransVoxel Solution...
REM del /q .\build.log
REM del /q .\msbuild.log

%msbuild% .\Solution\%SOLUTION%.sln /fileLogger /verbosity:m /t:rebuild /property:Configuration=Release > buildAllCpu.log
REM CPU Specific Build parameter
REM %msbuild% .\Solutions\%SOLUTION%.sln /fileLogger /verbosity:m /t:rebuild /property:Configuration=Release;Platform=x64 > buildx64.log
REM Build using Visual Studio
REM %devenv% .\Solutions\\%SOLUTION%.sln /Rebuild  "Release|x86" /Out .\build.log

rmdir /S /Q .\Binaries
mkdir .\Binaries
REM mkdir .\Binaries\x86
REM mkdir .\Binaries\x64

if exist ".\msbuild.log" move ".\msbuild.log" ".\Binaries\msbuild.log"
if exist ".\buildAllCpu.log" move ".\buildAllCpu.log" ".\Binaries\"

REM if exist ".\buildx64.log" move ".\buildx64.log" ".\Binaries\"
REM if exist ".\buildx86.log" move ".\buildx86.log" ".\Binaries\"

echo Gathering binaries...
REM csproj names in for loop
for %%F in ("Transvoxel") do (
echo Copying %%F
if exist ".\Projects\%%F\bin\Release\%%F.dll" copy ".\Projects\%%F\bin\Release\%%F.dll" .\Binaries\
if exist ".\Projects\%%F\bin\Release\%%F.pdb" copy ".\Projects\%%F\bin\Release\%%F.pdb" .\Binaries\
if exist ".\Projects\%%F\bin\Release\%%F.xml" copy ".\Projects\%%F\bin\Release\%%F.xml" .\Binaries\
)

echo Build Complete. See buildlog.txt for details. Binary files located in \Binaries\
pause