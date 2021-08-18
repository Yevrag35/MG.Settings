Import-Module "$PSScriptRoot\MG.Settings.Json.dll" -ea Stop;
$job = new-object Newtonsoft.Json.Linq.JObject
$job.Add((New-Object Newtonsoft.Json.Linq.JProperty('hi', 'bye')))

$ms = New-Object MG.Settings.Json.JsonSettings("C:\Users\Mike\AppData\Roaming\Mike Garvey\TranslatorDb", "appSettings.json", 
	[System.Text.Encoding]::UTF8, $job, $true)