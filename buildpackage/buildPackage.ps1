
function ReadLinesFromFile([string] $fileName)
{
 [string]::join([environment]::newline, (get-content -path $fileName))
}

function BuildSolution
{
  [CmdletBinding()]
  param()
  C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ..\MvcRouteTester.sln /t:build /p:Configuration=Debug
}

function GetLatestFullVersionOnNuget()
{
  [CmdletBinding()]
  param()

   $packageDetails = &nuget list MvcRouteTester
   $parts = $packageDetails.Split(' ')
   [string]$parts[1]
}

function GetLastVersionNumber()
{
  [CmdletBinding()]
  param()

  $fullVersion = GetLatestFullVersionOnNuget
  $parts = $fullVersion.Split('.')
  [int]$parts[2]
}

function GetNextVersionNumber
{
  [CmdletBinding()]
  param()
 (GetLastVersionNumber) + 1
}

function CleanupBuildArtifacts
{
  [CmdletBinding()]
  param()

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

# push to nuget:
$pushCommand = "NuGet Push MvcRouteTester.$fullVersion.nupkg"
Invoke-Expression $pushCommand
write-output "Pushed package version $fullVersion"

CleanupBuildArtifacts

write-output "Done"
