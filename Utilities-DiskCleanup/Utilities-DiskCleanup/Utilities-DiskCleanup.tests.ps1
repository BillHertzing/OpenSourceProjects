#
# This is a PowerShell Unit Test file for

# import the module that this pester tests file applies to
$ModuleUnderTest = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.ps1", ".psm1")
ipmo -force .\$ModuleUnderTest

function build-testdrive{
  param ($testdrive)
  New-Item -Path $TestDrive -Name "dir1" -ItemType Directory
  New-Item -Path $TestDrive -Name "dir2" -ItemType Directory
  Set-Content -Path (join-path $TestDrive 'file1') -Value "ipsolocum"
  Set-Content -Path (join-path (join-path $TestDrive 'dir2') 'file2') -Value 'ipsolocum'
}

Describe "Get-dirsubtree" {
  Context "Get-dirsubtree" {
    # Setup a test drive to use with the test
    build-testdrive $TestDrive

    # Tests
	  It "should throw an exception ParameterBindingValidationException when passed an empty string" {
		  {get-dirsubtree ''} | Should Throw "Cannot bind argument to parameter"
	  }
	  It "should return a hashtable when passed the testdrive" {
		  get-dirsubtree $testdrive | Should BeOftype System.Collections.Hashtable
	  }
  }
}

Describe "Get-FilehashHash" {
  Context "Get-FilehashHash" {
    # Setup a test drive to use with the test
    build-testdrive $TestDrive
    $fiprops = @('fullname','length','Hash')
    # build a dirsubtree from the testdrive
    $dirsubtree = get-dirsubtree $testdrive
    # Tests
	  #It "should throw an exception if called with no rootsubtree" {
	  #	{Get-FilehashHash} | Should Throw "Cannot bind argument to parameter"
	  #}
	  #It "should throw an exception if called without fiprops argument" {
	  #	{Get-FilehashHash $dirsubtree} | Should Throw "Cannot bind argument to parameter"
	  #}
	  It "should return a hashtable when passed a rootsubtree and the fiprops array" {
		  Get-FilehashHash $dirsubtree $fiprops | Should BeOftype System.Collections.Hashtable
	  }
  }
}