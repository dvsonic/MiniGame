@echo off
cd /d %~dp0

if not exist out ( mkdir out)
if not exist out\cs (mkdir out\cs)
if not exist out\lua (mkdir out\lua)
if not exist out\go\Message (mkdir out\go\Message)

echo ==========clear all generated files===============
del /F /S /Q out\*.cs
del /F /S /Q out\*.pb
del /F /S /Q out\*.lua
del /F /S /Q out\*.go
echo.

set PB_SRC_DIR=proto
echo ================Gen common Proto ======================
for %%i in (%PB_SRC_DIR%\*.proto) do (	
	tool\bin\protoc.exe -I=%PB_SRC_DIR% --csharp_out=out\cs %%i
	tool\bin\protoc.exe -I=%PB_SRC_DIR% --python_out=out\python %%i
    echo From %%i To %%~ni.cs Successfully!  
)
echo.
 
echo ===========Gen Msg Ids==============
tool\factory_gen.exe -oout\cs\msg_id.cs out\cs -pyout\python
echo.

set SRC_DIR=out
set DST_DIR_CS=E:\TestProject\MiniGame\Assets\Scripts\Net\Gen
set DST_DIR_PYTHON=E:\Project\Digital_Life_Server-master\proto

echo ========clear old files============
del /F /S /Q %DST_DIR_CS%\*.cs
echo.

echo ========== copy files ===================
xcopy /S /Q %SRC_DIR%\cs\*.cs %DST_DIR_CS%\
copy /Y %SRC_DIR%\python\AI_pb2.py %DST_DIR_PYTHON%\AI_pb2.py
copy /Y %SRC_DIR%\python\MessageID.py %DST_DIR_PYTHON%\MessageID.py
echo.
pause