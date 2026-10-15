param(
    [Parameter(Mandatory=$true)]
    [string]$vsixPath
)

$ErrorActionPreference = 'Stop'
Write-Host "Post-processing VSIX for SSMS (VsixV2 packaging): $vsixPath"

if (-not (Test-Path $vsixPath)) {
    throw "VSIX file not found: $vsixPath"
}

Add-Type -AssemblyName 'System.IO.Compression'
Add-Type -AssemblyName 'System.IO.Compression.FileSystem'

$zip = [System.IO.Compression.ZipFile]::Open($vsixPath, [System.IO.Compression.ZipArchiveMode]::Update)

try {
    # 1. Update extension.vsixmanifest: remove Prerequisites so it becomes a clean VsixV2 manifest
    $manifestEntry = $zip.GetEntry('extension.vsixmanifest')
    if ($manifestEntry) {
        $stream = $manifestEntry.Open()
        $reader = New-Object System.IO.StreamReader($stream, [System.Text.Encoding]::UTF8)
        $manifestXmlText = $reader.ReadToEnd()
        $reader.Dispose()
        $stream.Dispose()
        $manifestEntry.Delete()

        $xml = [xml]$manifestXmlText
        $nsmgr = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
        $nsmgr.AddNamespace('v', 'http://schemas.microsoft.com/developer/vsx-schema/2011')

        # Remove Prerequisites node
        $prereqsNode = $xml.SelectSingleNode('//v:Prerequisites', $nsmgr)
        if ($prereqsNode) {
            $prereqsNode.ParentNode.RemoveChild($prereqsNode) | Out-Null
            Write-Host "Removed <Prerequisites> node from extension.vsixmanifest."
        }

        # Write back extension.vsixmanifest as clean UTF-8
        $newManifestEntry = $zip.CreateEntry('extension.vsixmanifest', [System.IO.Compression.CompressionLevel]::Optimal)
        $utf8WithoutBom = New-Object System.Text.UTF8Encoding($false)
        $writer = New-Object System.IO.StreamWriter($newManifestEntry.Open(), $utf8WithoutBom)
        $xml.Save($writer)
        $writer.Dispose()
    }

    # 2. Delete manifest.json (converting package from VsixV3 to pure VsixV2 for SSMS)
    $manifestJsonEntry = $zip.GetEntry('manifest.json')
    if ($manifestJsonEntry) {
        $manifestJsonEntry.Delete()
        Write-Host "Removed manifest.json from VSIX package."
    }

    # 3. Delete catalog.json (converting package from VsixV3 to pure VsixV2 for SSMS)
    $catalogEntry = $zip.GetEntry('catalog.json')
    if ($catalogEntry) {
        $catalogEntry.Delete()
        Write-Host "Removed catalog.json from VSIX package."
    }

    # 4. Update LumiSqlFormatter.pkgdef: fix ToolsOptionsPages strings
    $pkgdefEntry = $zip.GetEntry('LumiSqlFormatter.pkgdef')
    if ($pkgdefEntry) {
        $stream = $pkgdefEntry.Open()
        $reader = New-Object System.IO.StreamReader($stream, [System.Text.Encoding]::UTF8)
        $pkgText = $reader.ReadToEnd()
        $reader.Dispose()
        $stream.Dispose()
        $pkgdefEntry.Delete()

        # Fix category display name
        $pkgText = $pkgText -replace '(\[\$RootKey\$\\ToolsOptionsPages\\Lumi SQL Formatter\]\r?\n)@="#0"', "`$1@=""Lumi SQL Formatter"""
        # Fix page display name
        $pkgText = $pkgText -replace '(\[\$RootKey\$\\ToolsOptionsPages\\Lumi SQL Formatter\\General\]\r?\n)@="#0"', "`$1@=""General"""

        $newPkgEntry = $zip.CreateEntry('LumiSqlFormatter.pkgdef', [System.IO.Compression.CompressionLevel]::Optimal)
        $utf8WithoutBom = New-Object System.Text.UTF8Encoding($false)
        $writer = New-Object System.IO.StreamWriter($newPkgEntry.Open(), $utf8WithoutBom)
        $writer.Write($pkgText)
        $writer.Dispose()
        Write-Host "Fixed ToolsOptionsPages display names in LumiSqlFormatter.pkgdef."
    }
} finally {
    $zip.Dispose()
}

Write-Host "VSIX post-processing complete (configured as pure VsixV2 SSMS extension)."
