#
# Get_FilehashHash.ps1
#

function Get-FilehashHash {
      [CmdletBinding()]
    param (
    [Parameter(Mandatory=$true, ValueFromPipeline = $true)]
    [object[]] $rootsubtree
    ,[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName  = $true)]
    [string[]] $fiprops
    ,[Parameter(Mandatory=$false, ValueFromPipelineByPropertyName  = $true)]
    [hashtable] $priorfilehash
    )
  Begin {
    #ToDo: validate that the rootdir object(s) passed have array of files and dirsubtree collections
    $filehash = @{}
    if ($PSBoundParameters.ContainsKey('priorfilehash')) {
      $filehash = $priorfilehash
    }
    function gethashes {
      param ($rootsubtree)
      # add every file in this hash to the full 
      foreach ($f in $rootsubtree.files) {
	      if ($filehash.ContainsKey($f.hash)) {
		      if ($filehash.($f.hash) -is [hashtable] ) {
			      ($filehash.($f.hash)).SizeOnDisk += $f.Length
            ($filehash.($f.hash)).files += ($f|Select-Object -Property $fiprops)
		      } else {
			      $filehash.($f.hash) = @{SizeOnDisk=($f.Length)*2;files=@(($filehash.($f.hash)),($f|Select-Object -Property $fiprops))}
		      }
	      } else {
		      $filehash.($f.hash) = $f|Select-Object -Property $fiprops
	      }
      }
      # get the hashes for the files in every subdirtree
      foreach ($subtree in $rootsubtree.dirsubtree) {
	     gethashes $subtree
      }
    }
  }

  Process {

    foreach ($subtree in $rootsubtree) {
     gethashes $subtree
    }
  }

  End {
    $filehash
  }
}
 