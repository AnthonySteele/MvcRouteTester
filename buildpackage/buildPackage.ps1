# params

param(
	[string]$push = "false",
	[string]$v = ""
)

# functions

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

   $packageList = &nuget list MvcRouteTester
   $packageDetails = $packageList[0]
   $lineParts = $packageDetails.Split(' ')
   [string]$lineParts[1]
}

function GetLastVersionNumber()
{
  [CmdletBinding()]
  param()

  $fullVersionString = GetLatestFullVersionOnNuget
  $versionParts = $fullVersionString.Split('.')
  $versionParts
}

function NextFullVersion()
{
  [CmdletBinding()]
  param()
  
  $parts = GetLastVersionNumber
  $lastPart = $parts[2]
  $newVersion = [int]$lastPart + 1 
  
  $parts[2] = [string]$newVersion
  
  $newVersion = [string]::Join(".", $parts)
  $newVersion
}

function CleanupBuildArtifacts
{
  [CmdletBinding()]
  param()

  del MvcRouteTester.nuspec
  del *.nupkg
}

# main script

BuildSolution

$fullVersion = $v
if ($fullVersion -eq "")
{
  $fullVersion = NextFullVersion
  write-output "Next package version from nuget: $fullVersion"
}
else
{
  write-output "Next package version from params: $fullVersion"
}


# make the nuspec file with the target version number
$nuspecTemplate = ReadLinesFromFile "MvcRouteTester.nuspec.template"
$nuspecWithVersion = $nuspecTemplate.Replace("#version#", $fullVersion)
$nuspecWithVersion > MvcRouteTester.nuspec

nuget pack MvcRouteTester.nuspec 
$pushCommand = "NuGet Push MvcRouteTester.$fullVersion.nupkg"


if ($push -eq "true")
{
  # push to nuget:
  Invoke-Expression $pushCommand
  write-output "Pushed package version $fullVersion"
}
else
{
 # dry run
  write-output "Dry run: specify '-push true' to push to nuget"
  write-output "Next package version: $fullVersion"
  write-output "Command is: $pushCommand"
}
CleanupBuildArtifacts

write-output "Done"
