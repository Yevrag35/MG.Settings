Import-Module "$PSScriptRoot\MG.Settings.Json.dll" -ea Stop;
$ms = New-Object MG.Settings.Json.Testing.MySettingManager