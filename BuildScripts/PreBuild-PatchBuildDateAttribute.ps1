Write-Output "==============================================================================================================================================="
Write-Output "Pre build. Patching BuildDate Attribute..."
Write-Output "==============================================================================================================================================="

$buildDateAttribute = [DateTime]::UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
Write-Output "Using: '$buildDateAttribute'"

$dir = Get-Location

# Write-Output "Currently in: '$dir'"

# Make a copy of the current 'Program.BuildDate.cs' file
Copy-Item -Path $dir\Program.BuildDate.cs -Destination $dir\Program.BuildDate.cs.orig
Write-Output "Original 'Program.cs' saved..."

# Rewrite the file, injecting the above timestamp
$fileLocation = Join-Path $dir -ChildPath "Program.BuildDate.cs" 
$placeHolder = "BUILD-DATE-ATTRIBUTE"
(get-content $fileLocation) | foreach-object {$_ -replace $placeHolder, $buildDateAttribute} | set-content $fileLocation

Write-Output "BuildDateAttribute written..."
Write-Output "==============================================================================================================================================="
