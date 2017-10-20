function Get-ChildItemRecurse {
<#
.Synopsis
  Does a recursive search through a PSDrive up to n levels.
.Description
  Does a recursive directory search, allowing the user to specify the number of
  levels to search.
.Parameter path
  The starting path.
.Parameter filepattern
  (optional) the search RegExp for matching files. RegExp
.Parameter levels
  The numer of levels to recurse.
.Example
  # Get up to three levels of files
  PS> Get-ChildItemRecurse *.* -levels 3

.Notes
  NAME:      Get-ChildItemRecurse
  AUTHOR:    tojo2000
	additional code extensions done by ATAP
#>
  Param([Parameter(Mandatory = $true,
                   ValueFromPipeLine = $true,
                   Position = 0)]
        [string]$path = '.',
        [Parameter(Mandatory = $false,
                   Position = 1,
                   ValueFromPipeLine = $true)]
        [string]$filepattern = '.*',
        [Parameter(Mandatory = $false,
                   Position = 2,
                   ValueFromPipeLine = $true)]
        [int]$levels = 0)

  if (-not (Test-Path $path)) {
    throw "$path is an invalid path."
  }

  $files = @(Get-ChildItem $path)
  $a = @()
  foreach ($file in $files) {
    if ($file -match $filepattern) {$a += $file}

    if ($file.PSIsContainer) {
      if ($levels -gt 0) {
        $a += @(Get-ChildItemRecurse -path $file.FullName -filepattern $filepattern -levels ($levels - 1))
      }
    }
  }
  #Return the (posibly empty) collection
  $a
}
