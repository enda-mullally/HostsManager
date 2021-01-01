Write-Output "==============================================================================================================================================="
Write-Output "Post build. Patching BuildDate Attribute..."
Write-Output "==============================================================================================================================================="

$dir = Get-Location

# Write-Output "Currently in: '$dir'"

# Restore originsl 'Program.cs' file
Copy-Item -Path $dir\Program.cs.orig -Destination $dir\Program.cs
Write-Output "Original 'Program.cs' recovered..."

del $dir\Program.cs.orig

Write-Output "==============================================================================================================================================="
