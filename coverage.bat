if not "%PROGRAMFILES(x86)%" == "" goto win64
if not "%ProgramFiles%" == "" goto win32
echo Cannot find program files environment variable
pause
goto end

:win32
"%ProgramFiles%\PartCover\PartCover .NET 4.0\partcover.exe" --settings PodcastUtilities.Common.Tests.PartcoverSettings.xml --output coverage.xml
"%ProgramFiles%\PartCover\PartCover .NET 4.0\partcover.browser.exe" --report coverage.xml
goto end

:win64
"%PROGRAMFILES(x86)%\PartCover\PartCover .NET 4.0\partcover.exe" --settings PodcastUtilities.Common.Tests.PartcoverSettings.64.xml --output coverage.xml
"%PROGRAMFILES(x86)%\PartCover\PartCover .NET 4.0\partcover.browser.exe" --report coverage.xml

:end
