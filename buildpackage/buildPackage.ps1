
function ReadLinesFromFile([string] $fileName)
{
 [string]::join([environment]::newline, (get-content -path $fileName))
}

function BuildSolution
{
  C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ..\MvcRouteTester.sln /t:build /p:Configuration=Debug
}

function GetLatestFullVersionOnNuget()
{
   $packageDetails = &nuget list MvcRouteTester
   $parts = $packageDetails.Split(' ')
   [string]$parts[1]
}

function GetLastVersionNumber()
{
  $fullVersion = GetLatestFullVersionOnNuget
  $parts = $fullVersion.Split('.')
  [int]$parts[2]
}

function GetNextVersionNumber
{
  $lastVer = GetLastVersionNumber
  $lastVer + 1
}

function CleanupBuildArtifacts
{
  del MvcRouteTester.nuspec
  del *.nupkg
}

BuildSolution

$nextVersionNumber = GetNextVersionNumber
$fullVersion = "1.0.$nextVersionNumber"
write-output "Next package version: $fullVersion"

# make the nuspec file with the target version number
$nuspecTemplate = ReadLinesFromFile "MvcRouteTester.nuspec.template"
$nuspecWithVersion = $nuspecTemplate.Replace("#version#", $fullVersion)
$nuspecWithVersion > MvcRouteTester.nuspec

nuget pack MvcRouteTester.nuspec 

$pushCommand = "NuGet Push MvcRouteTester.#version#.nupkg".Replace("#version#", $fullVersion)

# push to nuget:
Invoke-Expression $pushCommand
write-output "Pushed package version $nextVersion"

CleanupBuildArtifacts

write-output "Done"
