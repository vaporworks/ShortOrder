@echo off 
set OutDebugFile=output\ShortOrder.debug.js
set OutMinFile=output\ShortOrder.js
set ThirdPartyFile=output\ThirdParty.js
set AllFiles=
for /f "eol=] skip=1 delims=' " %%i in (source-references.js) do set Filename=%%i& call :Concatenate 

goto :Combine
:Concatenate 
    if /i "%AllFiles%"=="" ( 
        set AllFiles=..\%Filename:/=\%
    ) else ( 
        set AllFiles=%AllFiles% ..\%Filename:/=\%
    ) 
goto :EOF 

:Combine
type %AllFiles%                   > %OutDebugFile%.temp

@rem Now call Google Closure Compiler to produce a minified version
copy /y version-header.js %OutMinFile%
tools\curl -d output_info=compiled_code -d output_format=text -d compilation_level=ADVANCED_OPTIMIZATIONS --data-urlencode js_code@%OutDebugFile%.temp "http://closure-compiler.appspot.com/compile" > %OutMinFile%.temp

@rem Finalise each file by prefixing with version header and surrounding in function closure
copy /y version-header.js %OutDebugFile%
type %OutDebugFile%.temp		  >> %OutDebugFile%
del %OutDebugFile%.temp

copy /y version-header.js %OutMinFile%
type %OutMinFile%.temp		  	  >> %OutMinFile%
del %OutMinFile%.temp

xcopy %OutDebugFile% "..\js\ShortOrder.debug.js" /Y 
xcopy %OutMinFile% "..\js\ShortOrder.js" /Y