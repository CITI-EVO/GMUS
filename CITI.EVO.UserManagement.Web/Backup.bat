for %%* in (.) do set CURRDIR=%%~n*

set NOW=%TIME:~0,-3%
set NOW=%DATE:~-4%.%DATE:~3,2%.%DATE:~0,2%_%NOW% 
set NOW=%NOW::=.%
set NOW=%NOW: =%

"c:\Program Files\7-zip\7z.exe" a -r -t7z "C:\Backups\WebApps\%CURRDIR%_%NOW%.7z" *.*