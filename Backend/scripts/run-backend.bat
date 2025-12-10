@echo off
cd /d "%~dp0.."
echo Iniciando Backend...
dotnet run --project Backend.csproj
