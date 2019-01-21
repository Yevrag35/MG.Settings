Import-Module .\MG.Settings.JsonSettings.dll -ea Stop;
$cfg = New-Object MG.Settings.JsonSettings.ConfigManager("$desk\config.json");