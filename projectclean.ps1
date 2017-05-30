echo "!!!!!!!!!!!! CLEAN BIN !!!!!!!!!!!!!!!"

foreach($path in Get-ChildItem -Path .\ -Filter bin -Recurse -Directory -Name)
{
	echo $path
	rm -r $path
}


echo "!!!!!!!!!!!! CLEAN OBJ !!!!!!!!!!!!!!!"
foreach($path in Get-ChildItem -Path .\ -Filter obj -Recurse -Directory -Name)
{
	echo $path
	rm -r $path
}

