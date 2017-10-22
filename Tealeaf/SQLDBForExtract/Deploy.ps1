Write-Host "Starting '$DatabaseName' Database Deployment to '$DatabaseServer'"
$SqlCmdVarArguments = 'DatabaseName="' + $DatabaseName.Replace('"', '""') + '"'
$SqlCmd = 'sqlcmd.exe -E -S "' + $DatabaseServer + '" -i "TLWEB_Package.sql" -v ' + $SqlCmdVarArguments
'Executing: ' + $SqlCmd | Write-Host
Invoke-Expression $SqlCmd.Replace('"', '`"') | Write-Host
