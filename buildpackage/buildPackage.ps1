function ReadLinesFromFile([string] $fileName)
{
 [string]::join([environment]::newline, (get-content -path $fileName))
}

function GetNextVersionNumber
{
  $lastVersionText = get-content lastVersion.txt
  $lastVersion = [int]$lastVersionText
  $lastVersion + 1
}

function CleanupBuildArtifacts
{
  del MvcRouteTester.nuspec
  del *.nupkg
}

function UpdateVersionNumber([int] $newVersionNumber)
{
   $newVersionNumber > lastVersion.txt

  # write changed version number back to git
  git add lastVersion.txt
  git commit -m "automated package build and version number increment to $newVersionNumber"
  git push
}

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
write-output "Pushed package version $nextVersion";

CleanupBuildArtifacts
UpdateVersionNumber $nextVersionNumber

write-output "Done"
