
param (
    [string]$pack
 )

cd NugetOut
$packages = ls .

foreach($package in $packages)
{
    if($package.Name -eq $pack)
    {
    	echo $package.Name
	& "C:\Users\qba\soft\nuget.exe" push $package.Name -Source http://qoobaget.azurewebsites.net/nuget
    }
}

cd ..
