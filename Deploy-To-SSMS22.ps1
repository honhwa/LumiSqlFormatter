<#
.SYNOPSIS
    Script tự động cài đặt/triển khai Extension LumiSqlFormatter vào SSMS 22.
#>

param(
    [string]$SsmsExtensionsDir = "C:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\Extensions\LumiSqlFormatter",
    [string]$SsmsExePath = "C:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\SSMS.exe",
    [string]$Configuration = "Release"
)

$host.UI.RawUI.WindowTitle = "Cai dat Lumi SQL Formatter vao SSMS 22"

Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host "      DANG TIEN HANH CAI DAT LUMI SQL FORMATTER VAO SSMS 22" -ForegroundColor Cyan
Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host ""

# 0. Kiểm tra quyền Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "[CANH BAO] Ban dang chay khong co quyen Administrator." -ForegroundColor Yellow
    Write-Host "Vui long click chuot phai vao file 'Install-To-SSMS22.bat' va chon 'Run as administrator'." -ForegroundColor Yellow
    Read-Host "Nhan Enter de thoat..."
    exit 1
}

# 1. Tìm file VSIX
$vsixFile = Join-Path $PSScriptRoot "LumiSqlFormatter\bin\$Configuration\LumiSqlFormatter.vsix"
if (-not (Test-Path $vsixFile)) {
    $vsixFile = Join-Path $PSScriptRoot "LumiSqlFormatter\bin\Debug\LumiSqlFormatter.vsix"
}

if (-not (Test-Path $vsixFile)) {
    Write-Host "[LOI] Khong tim thay file LumiSqlFormatter.vsix tai:" -ForegroundColor Red
    Write-Host "  $vsixFile" -ForegroundColor Red
    Write-Host "`nVui long build solution truoc khi chay script nay." -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Nhan Enter de thoat..."
    exit 1
}

Write-Host "[1/5] Kiem tra va dong tien trinh SSMS neu dang chay..." -ForegroundColor Green
$ssmsList = Get-Process -Name "SSMS" -ErrorAction SilentlyContinue
if ($ssmsList) {
    Write-Host "      Phat hien SSMS dang chay. Dang dong de giai phong cac file dll..." -ForegroundColor Yellow
    $ssmsList | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
} else {
    Write-Host "      -> SSMS da dong." -ForegroundColor Gray
}

# 2. Tạo thư mục đích và giải nén toàn bộ VSIX (overwrite an toàn)
Write-Host "[2/5] Giai nen toan bo noi dung VSIX vao thu muc Extensions..." -ForegroundColor Green
Write-Host "      Dich: $SsmsExtensionsDir" -ForegroundColor Gray

try {
    if (-not (Test-Path $SsmsExtensionsDir)) {
        New-Item -ItemType Directory -Path $SsmsExtensionsDir -Force | Out-Null
    }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead($vsixFile)
    foreach ($entry in $zip.Entries) {
        if ([string]::IsNullOrWhiteSpace($entry.Name)) { continue }
        $destPath = Join-Path $SsmsExtensionsDir $entry.FullName
        $destDir = Split-Path $destPath -Parent
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $destPath, $true)
        Write-Host "      [OK] $entry" -ForegroundColor DarkGray
    }
    $zip.Dispose()
    Write-Host "      -> Giai nen thanh cong toan bo file (bao gom manifest.json, catalog.json, dll, pkgdef)." -ForegroundColor Green
}
catch {
    Write-Host "[LOI] Khong the giai nen file VSIX: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Chi tiet: $($_.Exception.ToString())" -ForegroundColor Red
    Write-Host ""
    Read-Host "Nhan Enter de thoat..."
    exit 1
}

# 3. Xóa ComponentModelCache của tất cả user profile để ép SSMS quét lại MEF
Write-Host "[3/5] Lam sach ComponentModelCache cua SSMS de nap component moi..." -ForegroundColor Green
try {
    $caches = Get-ChildItem -Path "C:\Users\*\AppData\Local\Microsoft\SSMS\*\ComponentModelCache" -Directory -ErrorAction SilentlyContinue
    if ($caches) {
        foreach ($c in $caches) {
            Remove-Item -Path "$($c.FullName)\*" -Recurse -Force -ErrorAction SilentlyContinue
            Write-Host "      [OK] Da xoa cache: $($c.FullName)" -ForegroundColor DarkGray
        }
    } else {
        Write-Host "      -> Khong tim thay cache can xoa." -ForegroundColor Gray
    }
}
catch {
    Write-Host "      [WARN] Khong the xoa mot so file cache: $($_.Exception.Message)" -ForegroundColor Yellow
}

# 4. Touch extensions.configurationchanged
Write-Host "[4/5] Danh dau cap nhat extension cache he thong..." -ForegroundColor Green
try {
    $parentDir = Split-Path $SsmsExtensionsDir -Parent
    $changeFile = Join-Path $parentDir "extensions.configurationchanged"
    Set-Content -Path $changeFile -Value (Get-Date).ToString() -Force -ErrorAction SilentlyContinue
    Write-Host "      -> Da tao extensions.configurationchanged." -ForegroundColor Gray
}
catch {
    Write-Host "      [WARN] Khong the ghi file extensions.configurationchanged: $($_.Exception.Message)" -ForegroundColor Yellow
}

# 5. Chạy SSMS /updateconfiguration
Write-Host "[5/5] Chay SSMS /updateconfiguration de hop nhat menu va registry..." -ForegroundColor Green
if (Test-Path $SsmsExePath) {
    try {
        $p = Start-Process -FilePath $SsmsExePath -ArgumentList "/updateconfiguration" -Wait -PassThru -NoNewWindow
        Write-Host "      -> SSMS /updateconfiguration da chay xong (ExitCode: $($p.ExitCode))." -ForegroundColor Green
    }
    catch {
        Write-Host "      [WARN] Khong the chay SSMS /updateconfiguration: $($_.Exception.Message)" -ForegroundColor Yellow
    }
} else {
    Write-Host "      [WARN] Khong tim thay SSMS.exe tai: $SsmsExePath" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host "   CAI DAT VA CAP NHAT HOAN TAT 100%!" -ForegroundColor Green
Write-Host ""
Write-Host "   CAC TINH NANG SAN SANG TRONG SSMS 22:" -ForegroundColor White
Write-Host "   1. Auto-Capitalization: In hoa Reserved Keywords, Types, Functions." -ForegroundColor White
Write-Host "   2. Suggestion Popup: Go 's', 'c', 'i'... popup danh sach snippet phong cach Redgate." -ForegroundColor White
Write-Host "   3. Quick Create: Boi den code trong editor, nhan Ctrl+Alt+S de tao snippet." -ForegroundColor White
Write-Host "   4. Custom Folder: Tools -> Options -> Lumi SQL Formatter de doi thu muc luu." -ForegroundColor White
Write-Host "====================================================================" -ForegroundColor Cyan
Write-Host ""
Read-Host "Nhan Enter de dong cua so nay..."
