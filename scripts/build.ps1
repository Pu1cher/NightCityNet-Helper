$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$output = Join-Path $repoRoot 'dist/NightCityNet-Helper'
$csc = Join-Path $env:WINDIR 'Microsoft.NET/Framework64/v4.0.30319/csc.exe'
if (-not (Test-Path -LiteralPath $csc)) { throw 'The Windows .NET Framework compiler was not found.' }
New-Item -ItemType Directory -Path $output -Force | Out-Null
$names = 'Core','App','Engine','Tests','Memory','Connection','Profanity','MouthPatch','Turbo','ModelCatalog','ReplyEndings','Studio','StudioUI'
$sources = $names | ForEach-Object { Join-Path $repoRoot ('src/' + $_ + '.cs') }
$compilerArgs = @('/nologo','/target:winexe','/platform:x64',('/out:' + (Join-Path $output 'PalcherBTW-NightCityNet-Helper.exe')), '/reference:System.Windows.Forms.dll','/reference:System.Drawing.dll','/reference:System.Web.Extensions.dll','/reference:System.IO.Compression.dll','/reference:System.IO.Compression.FileSystem.dll')
& $csc @compilerArgs @sources
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
foreach ($folder in 'Compatibility','Prompts','Models') {
    Copy-Item -LiteralPath (Join-Path $repoRoot ('assets/' + $folder)) -Destination $output -Recurse -Force
}
$shared = Join-Path $output 'Shared Setups'
New-Item -ItemType Directory -Path $shared -Force | Out-Null
Copy-Item -Path (Join-Path $repoRoot 'presets/*.ncnsetup.json') -Destination $shared -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'docs/user-guide.txt') -Destination (Join-Path $output 'README-English.txt') -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'docs/city-studio.txt') -Destination (Join-Path $output 'CITY-STUDIO-QUICKSTART.txt') -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'docs/MODEL-FILES.json') -Destination $output -Force
Write-Host ('Built: ' + $output)
