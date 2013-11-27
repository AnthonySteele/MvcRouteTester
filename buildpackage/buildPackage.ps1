
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

   $packageDetails = &nuget list MvcRouteTester.Mvc5
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

function CleanupBuildArtifacts
{
  [CmdletBinding()]
  param()

  del MvcRouteTester.Mvc5.nuspec
  del *.nupkg
}

BuildSolution

$nextVersionNumber = (GetLastVersionNumber) + 1
$fullVersion = "0.0.$nextVersionNumber"
write-output "Next package version: $fullVersion"

# make the nuspec file with the target version number
$nuspecTemplate = ReadLinesFromFile "MvcRouteTester.Mvc5.nuspec.template"
$nuspecWithVersion = $nuspecTemplate.Replace("#version#", $fullVersion)
$nuspecWithVersion > MvcRouteTester.Mvc5.nuspec

nuget pack MvcRouteTester.Mvc5.nuspec 

# push to nuget:
$pushCommand = "NuGet Push MvcRouteTester.Mvc5.$fullVersion.nupkg"
Invoke-Expression $pushCommand
write-output "Pushed package version $fullVersion"

CleanupBuildArtifacts

write-output "Done"
