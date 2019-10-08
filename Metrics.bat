REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

REM Remove any previous test execution files to prevent issues overwriting
IF EXIST "%~dp0TextToImage.trx" del "%~dp0TextToImage.trx%"

REM Remove any previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"

REM Run the tests against the targeted output
call :RunOpenCoverUnitTestMetrics

REM Generate the report output based on the test results
if %errorlevel% equ 0 ( 
 call :RunReportGeneratorOutput 
)

REM Launch the report
if %errorlevel% equ 0 ( 
 call :RunLaunchReport 
)
exit /b %errorlevel%

:RunOpenCoverUnitTestMetrics
"%USERPROFILE%\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"%VS160COMNTOOLS%\..\IDE\mstest.exe" ^
-targetargs:"/testcontainer:\"%~dp0\TextToImage.Tests\bin\Debug\netcoreapp2.2\TextToImage.Tests.dll\" /resultsfile:\"%~dp0TextToImage.trx\"" ^
-filter:"+[TextToImage*]* -[TextToImage.Tests]*" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0\GeneratedReports\TextToImageReport.xml"
exit /b %errorlevel%

:RunReportGeneratorOutput
"%USERPROFILE%\.nuget\packages\ReportGenerator\4.3.0\tools\net47\ReportGenerator.exe" ^
-reports:"%~dp0\GeneratedReports\TextToImageReport.xml" ^
-targetdir:"%~dp0\GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%

:RunLaunchReport
start "report" "%~dp0\GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%
