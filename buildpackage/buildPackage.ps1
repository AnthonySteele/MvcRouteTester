
# params

param([string]$v = "")


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

   $packageDetails = &nuget list $packageName
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

  del MvcRouteTester.MVC5.nuspec
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


 make the nuspec file with the target version number
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
