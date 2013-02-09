echo "Building the nuget package for MvcRouteTester."

del *.nupkg
nuget pack 

echo "Done."
pause

