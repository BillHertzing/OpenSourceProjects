#
# Get_Dirsubtree.ps1
#

# Use the File-Info module that automagically computes the hash of a file and extends the fileinfo type with the MD5 hash value
ipmo 'C:\Dropbox\whertzing\Visual Studio 2013\Projects\Modules\FileIO\FileIO\FileIO.psm1'

function get-dirsubtree{
    [CmdletBinding()]
    param (
    [Parameter(Mandatory=$true, ValueFromPipeline = $true)]
    [string[]] $rootdirnames
    )
    Begin {
        $dirsubtrees = @()
        function safe-getitems {
            param ($str)
            try {
                get-item $str -ErrorAction Stop
            } catch [System.Exception]{
                write-verbose ("The item {0} cannot be read" -f $str)
                # return null to the calling function 
                '';
            }
        }

        function GetDirsubtree {
            param ($rootdir)
            $o = @{files=@();dirsubtree=@()}
            $d=''
            if ($rootdir -is [System.IO.DirectoryInfo]) {
                $o.rootdirname = $rootdir.fullname
                $d=$rootdir
            } else {
                $o.rootdirname = $rootdir
                $d = safe-getitems $rootdir
                if (-not $d) {$o;return}
            }
            foreach ($f in $d.GetFiles()) {
                # Check for exceptions like unautherized access, junctions, 
                # add the fileinfo structure to this rootdir's files key
                $o.files += $f
            }

            foreach ($subdir in $d.GetDirectories()){
                # Check for exceptions like unautherized access, junctions, 
                $o.dirsubtree += GetDirsubtree $subdir
          }
          $o
        }
    }
    Process {
        foreach ($rootdirname in $rootdirnames) {
            $d = safe-getitems $rootdirname
            # each $d will be of type System.IO.DirInfo
            if ($d) {$dirsubtrees += GetDirsubtree $d}
        }
    }
    END{
        # return the final dirsubtrees array
        $dirsubtrees
    }
}

