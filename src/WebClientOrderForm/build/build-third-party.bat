@echo off
set ThirdPartyFile=output\ThirdParty.js
set AllFiles=
for /f "eol=] skip=1 delims=' " %%i in (third-party-libs.js) do set Filename=%%i& call :Concatenate

goto :Combine
:Concatenate 
    if /i "%AllFiles%"=="" ( 
        set AllFiles=..\%Filename:/=\%
    ) else ( 
        set AllFiles=%AllFiles% ..\%Filename:/=\%
    ) 
goto :EOF 

:Combine
type %AllFiles%                   > %ThirdPartyFile%

xcopy %ThirdPartyFile% "..\js\ThirdParty.js" /Y