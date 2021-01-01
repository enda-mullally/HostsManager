Write-Output "==============================================================================================================================================="
Write-Output "Post build. Patching BuildDate Attribute..."
Write-Output "==============================================================================================================================================="

$dir = Get-Location

# Write-Output "Currently in: '$dir'"

# Restore originsl 'Program.BuildDate.cs' file
Copy-Item -Path $dir\Program.BuildDate.cs.orig -Destination $dir\Program.BuildDate.cs
Write-Output "Original 'Program.BuailDate.cs' recovered..."

del $dir\Program.BuildDate.cs.orig

Write-Output "==============================================================================================================================================="
