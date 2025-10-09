@echo off
chcp 65001 >nul
setlocal EnableDelayedExpansion

rem === 配置 ===
set "SRC=Assets"
set "DST=Docs"
set /a GROUPSIZE=200
rem =============

rem 检查目录
if not exist "%SRC%" (
    echo Source folder "%SRC%" not found.
    goto END
)
if not exist "%DST%" md "%DST%"

rem 统计文件数
set /a TOTAL=0
for /r "%SRC%" %%F in (*.cs) do set /a TOTAL+=1
if %TOTAL%==0 (
    echo No .cs file found.
    goto END
)
echo Found %TOTAL% .cs files.
echo.

rem 开始复制并分组
set /a DONE=0
set /a COUNT=0
set /a DIRIDX=1
set "CURDIR=%DST%\1"
md "%CURDIR%" >nul 2>&1
echo Copying to "%CURDIR%" ...

for /r "%SRC%" %%F in (*.cs) do (

    if !COUNT! GEQ %GROUPSIZE% (
        set /a COUNT=0
        set /a DIRIDX+=1
        set "CURDIR=%DST%\!DIRIDX!"
        md "!CURDIR!" >nul 2>&1
        echo ---- switch to "!CURDIR!" ----
    )

    copy /y "%%F" "!CURDIR!\%%~nxF.txt" >nul
    set /a COUNT+=1
    set /a DONE+=1
    set /a PCT=DONE*100/TOTAL
    echo [!DONE!/!TOTAL! - !PCT!%%]  %%~nxF.txt
)

echo.
echo DONE.
pause
:END
