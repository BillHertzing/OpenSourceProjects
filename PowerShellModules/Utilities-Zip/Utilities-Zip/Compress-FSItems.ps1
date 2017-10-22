
# Looks like the code for New-7ZipFile is from Zachary Loeber recorded here https://github.com/zloeber/Powershell/blob/master/Supplemental/New-ZipFile.ps1
function New-7ZipFile {
  [CmdletBinding()]
  param(
    # The path of the zip to create
    [Parameter(ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
    [Alias("FN")]
    $Path,

    # Items that we want to add to the ZipFile
    [Parameter(Position=1, Mandatory=$true, ValueFromPipelineByPropertyName=$true)]
    [Alias("PSPath","Item")]
    [string[]]$InputObject = $Pwd,

    # Append to an existing zip file, instead of overwriting it
    [Switch]$Append,

    # The compression level (defaults to Optimal):
    #   Optimal - The compression operation should be optimally compressed, even if the operation takes a longer time to complete.
    #   Fastest - The compression operation should complete as quickly as possible, even if the resulting file is not optimally compressed.
    #   NoCompression - No compression should be performed on the file.
    [System.IO.Compression.CompressionLevel]$Compression = "Optimal"
  )
  begin {
    # Make sure the folder already exists
    [string]$File = Split-Path $ZipFilePath -Leaf
    [string]$Folder = $(if($Folder = Split-Path $ZipFilePath) { Resolve-Path $Folder } else { $Pwd })
    $ZipFilePath = Join-Path $Folder $File
    # If they don't want to append, make sure the zip file doesn't already exist.
    if(!$Append) {
      if(Test-Path $ZipFilePath) { Remove-Item $ZipFilePath }
    }
    $Archive = [System.IO.Compression.ZipFile]::Open( $ZipFilePath, "Update" )
  }
  process {
    foreach($path in $InputObject) {
      foreach($item in Resolve-Path $path) {
        # Push-Location so we can use Resolve-Path -Relative 
        Push-Location (Split-Path $item)
        # This will get the file, or all the files in the folder (recursively)
        foreach($file in Get-ChildItem $item -Recurse -File -Force | % FullName) {
          # Calculate the relative file path
          $relative = (Resolve-Path $file -Relative).TrimStart(".\")
          # Add the file to the zip
          $null = [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($Archive, $file, $relative, $Compression)
        }
        Pop-Location
      }
    }
  }
  end {
    $Archive.Dispose()
    Get-Item $ZipFilePath
  }
}


function Expand-7ZipFile {
  #.Synopsis
  #  Expand a zip file, ensuring it's contents go to a single folder ...
  [CmdletBinding()]
  param(
    # The path of the zip file that needs to be extracted
    [Parameter(ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
    [Alias("FN")]
    $Path,

    # The path where we want the output folder to end up
    [Parameter(Position=1)]
    $OutputPath = $Pwd

  )
#region FunctionBeginBlock
BEGIN {
if (-not (test-path "$env:ProgramFiles\7-Zip\7z.exe")) {throw "$env:ProgramFiles\7-Zip\7z.exe needed"}
set-alias sz "$env:ProgramFiles\7-Zip\7z.exe"
$outputOption = "-o$OutputPath"
}
#endregion FunctionBeginBlock

#region FunctionProcessBlock
PROCESS {}
#endregion FunctionProcessBlock

#region FunctionEndBlock
END {
sz x "$Path" $outputOption -r -y
}
#endregion FunctionEndBlock
 } 
 
 # The code foor new-zipfile and expand-zipfile can be found on a number of blog posts, repositories, and gists
 # Jaykul had one in 2014 at https://github.com/PoshCode/PoshCode/blob/master/Installation.psm1
 ###############################################################################
## Copyright (c) 2013 by Joel Bennett, all rights reserved.
## Free for use under MS-PL, MS-RL, GPL 2, or BSD license. Your choice. 
###############################################################################

 Add-Type -As System.IO.Compression.FileSystem

function New-ZipFile {
  #.Synopsis
  #  Expand a zip file, ensuring it's contents go to a single folder ...
  [CmdletBinding()]
  param(
    # The path of the zip to create
    [Parameter(Position=0, Mandatory=$true)]
    $ZipFilePath,

    # Items that we want to add to the ZipFile
    [Parameter(Position=1, Mandatory=$true, ValueFromPipelineByPropertyName=$true)]
    [Alias("PSPath","Item")]
    [string[]]$InputObject = $Pwd,

    # Append to an existing zip file, instead of overwriting it
    [Switch]$Append,

    # The compression level (defaults to Optimal):
    #   Optimal - The compression operation should be optimally compressed, even if the operation takes a longer time to complete.
    #   Fastest - The compression operation should complete as quickly as possible, even if the resulting file is not optimally compressed.
    #   NoCompression - No compression should be performed on the file.
    [System.IO.Compression.CompressionLevel]$Compression = "Optimal"
  )
  begin {
    # Make sure the folder already exists
    [string]$File = Split-Path $ZipFilePath -Leaf
    [string]$Folder = $(if($Folder = Split-Path $ZipFilePath) { Resolve-Path $Folder } else { $Pwd })
    $ZipFilePath = Join-Path $Folder $File
    # If they don't want to append, make sure the zip file doesn't already exist.
    if(!$Append) {
      if(Test-Path $ZipFilePath) { Remove-Item $ZipFilePath }
    }
    $Archive = [System.IO.Compression.ZipFile]::Open( $ZipFilePath, "Update" )
  }
  process {
    foreach($path in $InputObject) {
      foreach($item in Resolve-Path $path) {
        # Push-Location so we can use Resolve-Path -Relative 
        Push-Location (Split-Path $item)
        # This will get the file, or all the files in the folder (recursively)
        foreach($file in Get-ChildItem $item -Recurse -File -Force | % FullName) {
          # Calculate the relative file path
          $relative = (Resolve-Path $file -Relative).TrimStart(".\")
          # Add the file to the zip
          $null = [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($Archive, $file, $relative, $Compression)
        }
        Pop-Location
      }
    }
  }
  end {
    $Archive.Dispose()
    Get-Item $ZipFilePath
  }
}


function Expand-ZipFile {
  #.Synopsis
  #  Expand a zip file, ensuring it's contents go to a single folder ...
  [CmdletBinding()]
  param(
    # The path of the zip file that needs to be extracted
    [Parameter(ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
    [Alias("PSPath")]
    $FilePath,

    # The path where we want the output folder to end up
    [Parameter(Position=1)]
    $OutputPath = $Pwd,

    # Make sure the resulting folder is always named the same as the archive
    [Switch]$Force
  )
  process {
    $ZipFile = Get-Item $FilePath
    $Archive = [System.IO.Compression.ZipFile]::Open( $ZipFile, "Read" )

    # Figure out where we'd prefer to end up
    if(Test-Path $OutputPath) {
      # If they pass a path that exists, we want to create a new folder
      $Destination = Join-Path $OutputPath $ZipFile.BaseName
    } else {
      # Otherwise, since they passed a folder, they must want us to use it
      $Destination = $OutputPath
    }

    # The root folder of the first entry ...
    $ArchiveRoot = ($Archive.Entries[0].FullName -Split "/|\\")[0]

    Write-Verbose "Desired Destination: $Destination"
    Write-Verbose "Archive Root: $ArchiveRoot"

    # If any of the files are not in the same root folder ...
    if($Archive.Entries.FullName | Where-Object { @($_ -Split "/|\\")[0] -ne $ArchiveRoot }) { 
      # extract it into a new folder:
      New-Item $Destination -Type Directory -Force
      [System.IO.Compression.ZipFileExtensions]::ExtractToDirectory( $Archive, $Destination )
    } else {
      # otherwise, extract it to the OutputPath 
      [System.IO.Compression.ZipFileExtensions]::ExtractToDirectory( $Archive, $OutputPath )

      # If there was only a single file in the archive, then we'll just output that file...
      if($Archive.Entries.Count -eq 1) {
        # Except, if they asked for an OutputPath with an extension on it, we'll rename the file to that ... 
        if([System.IO.Path]::GetExtension($Destination)) {
          Move-Item (Join-Path $OutputPath $Archive.Entries[0].FullName) $Destination
        } else {
          Get-Item (Join-Path $OutputPath $Archive.Entries[0].FullName)
        }
      } elseif($Force) {
        # Otherwise let's make sure that we move it to where we expect it to go, in case the zip's been renamed
        if($ArchiveRoot -ne $ZipFile.BaseName) {
          Move-Item (join-path $OutputPath $ArchiveRoot) $Destination
          Get-Item $Destination
        }
      } else {
        Get-Item (Join-Path $OutputPath $ArchiveRoot)
      }
    }

    $Archive.Dispose()
  }
}

<#
.SYNOPSIS 
Zips upspecified logfiles on remote computers, uses Powershell remoting
.DESCRIPTION
The function Zip-Logfiles will read the tealeaf registry and cfg files for the locations of logs
    identifies the logs that match a specified date range and pattern
		and zips them into a specified output filee
Provide Get-AllRegKey a list of computernames, and it will remote to those computers
    and perform the smae operations on the remote computer

  
.PARAMETER ComputerNames
In the Zip-Logfiles function this is a single computer name or an array of computer names
.EXAMPLE
C:\PS> Zip-Logfiles
.EXAMPLE
C:\PS> Zip-Logfiles -ComputerName @('localhost','RemoteCN') 
.EXAMPLE
C:\PS> Zip-Logfiles -ComputerName 
#>


function Compress-Logfiles {
#region FunctionCommentBlock
<#
#.Synopsis 
#  Create a script block that zips up log files, then either execute that script block locally or send it to a remote computer for execution there
#.Parameter SettingsFile
#  Name of a file that contains settings for the script
#.Example
# Zip-Logfiles
# Zip-Logfiles -SettingsFile SettingsFileName
#
#.Attribution
# Written by Bill Hertzing between 2011 and 7/1/2013

#>
#endregion FunctionCommentBlock

#region FunctionParameters
[CmdletBinding()]
Param (
  [parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $ComputerName  
  ,[parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $InFnPath  
  ,[parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $InFnPattern  
  ,[parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $OutFnPath  
  ,[parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $FromDateTime  
  ,[parameter(ValueFromPipeline=$True, ValueFromPipelineByPropertyName=$True)] $ToDateTime  
  ,[parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)] $settingsFile
  ,[parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)] $LoggingObject
) 
#endregion FunctionParameters

#region FunctionBeginBlock
BEGIN {
########################################
# Start gathering the statistics about this execution
$results = @{};
$results.RunTimeStart=Get-Date;

########################################
#	Support loading of scripts and modules from the same dir where the running script resides
#	Make it possible to find files relative to the directory in which the "invoking script" resides
# Get the external script file name for use in the log filename
$cs = 0;while (((Get-Variable MyInvocation -Scope $cs).Value).MyCommand.CommandType -ne 'ExternalScript') {$cs++}
$invokingScriptsPath = (((Get-Variable MyInvocation -Scope $cs).Value).MyCommand.Path | split-path)+'\'
$invokingScriptsName = [system.io.path]::getfilenamewithoutextension(((Get-Variable MyInvocation -Scope $cs).Value).MyCommand.Name)

# The settings
$settings = @{
  InFnPath = ''
  #	Input File Name Pattern
	InFnPattern = '.+_DS_.+\.(log|txt)$'
  #	Output File Path Pattern
  OutFnPath =  ''
	#	Output File Name Pattern (leave null if populated programaticaly)
	OutFnPattern = '{0}_{1}_ConsolidatedLogs.zip' ;
	ComputerName= @("$env:computername")

	# Specific Settings 
  #FromDateTime = [DateTime] '12/26/2012'
  #FromDateTime = [DateTime] '07/01/2013 15:00:00'
  #ToDateTime =   [DateTime] '07/01/2013 17:00:00'
  FromDateTime = (get-date).adddays(-41)
  ToDateTime = (get-date).addhours(-2)

	ZipPassword = 'AcdF23gH!s@3'
}

# Process Settings Files and parameter overrides
ipmo Utilities-Settings -force
Get-Settings $SettingsFile

# Powershell functions (especially within Modules) must inherit the logging object
# If the $LoggingObject is not null, maake sure it is valid
try {if ($LoggingObject) {ipmo Init-Log4Net; LogIt "starting";}}
catch {	throw "Can't log to non-null LoggingObject"}

# Things to be initialized after settings are processed
if ($ComputerName) {$Settings.ComputerName = $ComputerName}
if ($InFnPath) {$Settings.InFnPath = $InFnPath}
if ($InFnPattern) {$Settings.InFnPattern = $InFnPattern}
if ($OutFnPath) {$Settings.OutFnPath = $OutFnPath}
if ($FromDateTime) {$Settings.FromDateTime = $FromDateTime}
if ($ToDateTime) {$Settings.ToDateTime = $ToDateTime}


######
#Variables
# Collection to hold processing status flags and messages for each computername
$a = @{}
# populate the collection (hash) with initial keys of all the computernames
foreach ($cn in $Settings.computername) {
	$a.Add($cn, @{
		zipstarted = $false
		zipresults = $false
		zipcompleted = $false
		movestarted = $false
		movecompleted = $false
		unzipstarted = $false
		unzipcompleted = $false
	})
}

#######
#Local Functions
	function Test-PsRemoting 
	{ 
	    param([Parameter(Mandatory = $true)] $computername ) 
	    try { 
	        $errorActionPreference = "Stop" 
	        $result = Invoke-Command -ComputerName $computername { 1 } 
	    } catch { 
	        Write-Verbose $_ 
	        return $false 
	    } 
	    $true    
	}
	
	# ensure PSRemoting will work to each computer
	foreach ($cn in $Settings.computername) {
		if ((Test-PsRemoting $cn) -ne $true) {
			# ToDo: report that TealeafNotInstalled
			"PSRemoting Not Allowed to computer $cn"
			return
		}
	}

}
#endregion FunctionBeginBlock

#region FunctionProcessBlock
PROCESS {
	# Iterate over each computername, and zip-move-unzip
	foreach ($cn in $Settings.computername) {

		# Create the script block to be executed on this specific computername
		#  This is  remoteable
		$_ZipLogFilesSB = {Param($FromDateTime, $ToDateTime, $InFnPattern, $OutFnPattern, $InFnPath, $OutFnPath)
		 	# Create a local named function
		  function _ZipLogFiles {
		    Param($FromDateTime, $ToDateTime, $InFnPattern, $OutFnPattern, $InFnPath, $OutFnPath)
				#Local varialbes:
				$InFnPath = '';
				# Create a function that reads the location of hte log files from the registry
				function Get-LogFnPath {
			    try {
		        $InFnPath = (get-itemproperty  -path 'Registry::HKLM\software\tealeaf technology' ).TeaLeafLogDir + '\';
			    } catch [Exception] {
		        try {
		            $InFnPath = (get-itemproperty  -path 'Registry::HKLM\software\wow6432node\tealeaf technology' ).TeaLeafLogDir + '\';
		        }catch [Exception] {
						  # ToDo: report that TealeafNotInstalled
							"Tealeaf Not Installed on computer $cn"
		        }
		    	}
					$ErrorActionPreference = $EAP
				}
				# A function that does the work of zipping
				function Add-Zip {
				    param($outzipfn, $LogFnsToZip)
						# Create an empty zip file with teh magic header
				    if(-not (test-path($outzipfn)))
				    {
				        set-content $outzipfn ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
				        (dir $outzipfn).IsReadOnly = $false  
				    }

				    $shellApplication = new-object -com shell.application
				    $zipPackage = $shellApplication.NameSpace($outzipfn)

				    foreach($fn in $LogFnsToZip) 
				    { 
				            $zipPackage.CopyHere($fn.FullName)
				            Start-sleep -seconds 3
				    }
				}
				# ScriptBlockStartsHere
				# Set the ErrorActionPrefernce for this ScriptBlock
				$ErrorActionPreference = "SilentlyContinue"

				# Validate we can write the output file, create an empty zip (remove any that already exists)
				$outzipfn = $OutFnPath + $OutFnPattern
				try {new-item $outzipfn -force -type file >$null}
				catch {
				  # ToDo: report that CannotCreateOutputFile
					"Cannot Create $outzipfn on computer $cn"
				}
        set-content $outzipfn ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
        (dir $outzipfn).IsReadOnly = $false  

				# Get the location of the log files (the log directory from the registry)
				# Get the location of the log files (the log directory from the transport cfg files)
				# Get the list of  log files matching the input patterns bounded by the dates
				$InFnPath = Get-LogFnPath
				# Verify at least one file matching the input pattern exists. force the results into an array. 
				$files=@(get-childitem $InFnPath | where {($_.Name -match $InFnPattern) -and ($true)} )
				if ($files.Count -eq 0) {
				  # ToDo: report that NoMatchingLogFilesPresent
					"No Input file matching $InFnPattern written between $FromDateTime and $ToDateTime was found in directory $InFnPath on computer $cn"
					return
				}
				# Zip the files into the output
				Add-Zip $outzipfn $files
			}
		  # Call the local named function
		  &_ZipLogFiles $FromDateTime, $ToDateTime, $InFnPattern, $OutFnPattern, $InFnPath, $OutFnPath
		}

		# Create a callback function on this computer to be executed when the script block on the remote computer returns
		# Execute the scriptblock (either locally or remote), and specify the callback function
	  ($a.$cn).zipstarted=$true
		switch ($cn) {
      # If the computer name is localhost, or the same name as hostname
      #   use the Call operator to call the scriptblock, and assign the array returned 
      #   to the hash using the current computername as the key
      {$_ -match "localhost|" + (hostname)} {
        $a.$cn.zipresults = &$_ZipLogFilesSB $settings.FromDateTime $settings.ToDateTime $settings.InFnPattern $settings.OutFnPattern 
        break
      }
      # for all other computer names, execute the command remotely using invoke-command
      default {
				# ToDo: Validate that we can reach Powershell Remoting on the computer
        # pass the scriptblock to the remote computers
        #  assign the array returned to the hash using the current computername as the key
        $a.$cn.zipresults  = (invoke-command -Scriptblock $_ZipLogFilesSB `
            -ArgumentList $settings.FromDateTime $settings.ToDateTime $settings.InFnPattern $settings.OutFnPattern -computername "$cn")
      }
    }
	}

	}

#endregion FunctionProcessBlock

#region FunctionEndBlock
END {
}
#endregion FunctionEndBlock
}

# 7ZipBackup
#https://7zbackup.codeplex.com
# Under GPLv2
# part of the module