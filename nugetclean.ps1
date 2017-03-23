if((Test-Path -Path NugetOut ))
{
    rm -r "NugetOut"
}


foreach($path in Get-ChildItem -Path .\ -Filter *.nupkg -Recurse -File -Name)
{
	echo $path
	rm $path
}

