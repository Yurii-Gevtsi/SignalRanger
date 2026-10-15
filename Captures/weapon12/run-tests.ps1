param([string]$RunName, [string]$TestPlatform = 'EditMode', [string]$TestFilter = 'Game.Tests.EditMode.Gameplay.BattleSimulationTests.CanonicalBarrier')
$ErrorActionPreference = 'Stop'
$outputDirectory = 'C:\Projects\Game\Captures\weapon12'
$resultPath = Join-Path $outputDirectory "$RunName.xml"
$logPath = Join-Path $outputDirectory "$RunName.log"
$unityArguments = @('-batchmode', '-nographics', '-projectPath', 'C:\Projects\Game', '-runTests', '-testPlatform', $TestPlatform, '-testFilter', $TestFilter, '-testResults', $resultPath, '-logFile', $logPath)
$unityProcess = Start-Process 'C:\Program Files\Unity\Hub\Editor\6000.5.1f1\Editor\Unity.exe' -ArgumentList $unityArguments -WindowStyle Hidden -PassThru
$unityProcess.WaitForExit()
if (!(Test-Path -LiteralPath $resultPath)) { Write-Output "No result XML. Unity exit $($unityProcess.ExitCode). See $logPath"; exit 1 }
[xml]$results = Get-Content -LiteralPath $resultPath -Raw
$cases = $results.SelectNodes('//test-case')
$failed = @($cases | Where-Object { $_.result -ne 'Passed' })
Write-Output "$RunName : $($cases.Count) tests; $($failed.Count) non-passing; Unity exit $($unityProcess.ExitCode)"
foreach ($case in $failed) { Write-Output "$($case.fullname): $($case.failure.message)" }
if ($cases.Count -eq 0 -or $failed.Count -gt 0 -or $unityProcess.ExitCode -ne 0) { exit 1 }
exit 0
