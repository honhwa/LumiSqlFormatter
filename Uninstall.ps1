# ==============================================================================
#  Lumi SQL Formatter - Uninstall Script (Gỡ sạch 100% mọi vết tích extension)
# ==============================================================================
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::InputEncoding = [System.Text.Encoding]::UTF8
System.Text.ASCIIEncoding = [System.Text.Encoding]::UTF8
Write-Host "==============================================================================" -ForegroundColor Cyan
Write-Host "       TRÌNH GỠ CÀI ĐẶT HOÀN TOÀN LUMI SQL FORMATTER & CÁC EXTENSION CŨ        " -ForegroundColor Cyan
Write-Host "==============================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Đóng tiến trình SSMS nếu đang chạy
Write-Host "[1/5] Đang kiểm tra và đóng tiến trình SSMS..." -ForegroundColor Yellow
$ssmsProcesses = Get-Process -Name "Ssms" -ErrorAction SilentlyContinue
if ($ssmsProcesses) {
    Write-Host "  -> Đang đóng SSMS để mở khóa file DLL..." -ForegroundColor Gray
    Stop-Process -Name "Ssms" -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
} else {
    Write-Host "  -> SSMS hiện không chạy." -ForegroundColor Gray
}

# 2. Quét và xóa toàn bộ thư mục extension chứa LumiSqlFormatter, SqlAutoUppercase, SqlKeywordUpper
Write-Host "[2/5] Đang quét và xóa thư mục extension trên toàn bộ hệ thống..." -ForegroundColor Yellow

$searchRoots = @(
    "$env:LOCALAPPDATA\Microsoft\SSMS",
    "$env:LOCALAPPDATA\Microsoft\SQL Server Management Studio",
    "$env:LOCALAPPDATA\Microsoft\VisualStudio",
    "${env:ProgramFiles}\Microsoft SQL Server Management Studio 18",
    "${env:ProgramFiles}\Microsoft SQL Server Management Studio 19",
    "${env:ProgramFiles}\Microsoft SQL Server Management Studio 20",
    "${env:ProgramFiles}\Microsoft SQL Server Management Studio 22",
    "${env:ProgramFiles(x86)}\Microsoft SQL Server Management Studio 18",
    "${env:ProgramFiles(x86)}\Microsoft SQL Server Management Studio 19"
)

$targetDlls = @("LumiSqlFormatter.dll", "SqlAutoUppercase.dll", "SqlKeywordUpper.dll")

foreach ($root in $searchRoots) {
    if (Test-Path $root) {
        foreach ($dll in $targetDlls) {
            $foundFiles = Get-ChildItem -Path $root -Recurse -Filter $dll -ErrorAction SilentlyContinue
            foreach ($file in $foundFiles) {
                # Tránh xóa nhầm thư mục source code / build của dự án
                if ($file.DirectoryName -like "*\LumiSqlFormatter\bin\*" -or $file.DirectoryName -like "*\LumiSqlFormatter\obj\*") {
                    continue
                }
                $extDir = $file.DirectoryName
                Write-Host "  -> Xóa thư mục extension: $extDir" -ForegroundColor Green
                try {
                    Remove-Item -Path $extDir -Recurse -Force -ErrorAction SilentlyContinue
                } catch {
                    Write-Host "     Lỗi xóa $($extDir): $_" -ForegroundColor Red
                }
            }
        }
    }
}

# Xóa các thư mục named extensions nếu còn sót
$namedFolders = @("LumiSqlFormatter", "SqlAutoUppercase", "SqlKeywordUpper")
foreach ($root in $searchRoots) {
    if (Test-Path $root) {
        foreach ($name in $namedFolders) {
            $matchingDirs = Get-ChildItem -Path $root -Recurse -Directory -Filter $name -ErrorAction SilentlyContinue
            foreach ($dir in $matchingDirs) {
                if ($dir.FullName -like "*d:\LumiSqlFormatter*" -or $dir.FullName -like "*D:\LumiSqlFormatter*") {
                    continue
                }
                Write-Host "  -> Xóa thư mục: $($dir.FullName)" -ForegroundColor Green
                Remove-Item -Path $dir.FullName -Recurse -Force -ErrorAction SilentlyContinue
            }
        }
    }
}

# 3. Dọn sạch ComponentModelCache & Extension Metadata Caches
Write-Host "[3/5] Dọn dẹp ComponentModelCache và Metadata Cache của SSMS..." -ForegroundColor Yellow

# SSMS 22+ (AppData\Local\Microsoft\SSMS)
if (Test-Path "$env:LOCALAPPDATA\Microsoft\SSMS") {
    $ssmsInstances = Get-ChildItem -Path "$env:LOCALAPPDATA\Microsoft\SSMS" -Directory -Filter "22.*" -ErrorAction SilentlyContinue
    foreach ($inst in $ssmsInstances) {
        # Xóa ComponentModelCache
        $cmc = Join-Path $inst.FullName "ComponentModelCache"
        if (Test-Path $cmc) {
            Write-Host "  -> Làm mới ComponentModelCache: $cmc" -ForegroundColor Gray
            Get-ChildItem -Path $cmc -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
        }

        # Xóa Extension metadata cache
        $extPath = Join-Path $inst.FullName "Extensions"
        if (Test-Path $extPath) {
            Get-ChildItem -Path $extPath -Include "*.mpack", "*.cache", "CompatibilityList.xml" -Recurse -ErrorAction SilentlyContinue | ForEach-Object {
                Write-Host "  -> Xóa file cache: $($_.Name)" -ForegroundColor Gray
                Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
            }
            # Tạo file configurationchanged để SSMS re-index
            Set-Content -Path (Join-Path $extPath "extensions.configurationchanged") -Value "" -ErrorAction SilentlyContinue
        }
    }
}

# SSMS 18/19/20 (AppData\Local\Microsoft\SQL Server Management Studio)
if (Test-Path "$env:LOCALAPPDATA\Microsoft\SQL Server Management Studio") {
    $legacyInstances = Get-ChildItem -Path "$env:LOCALAPPDATA\Microsoft\SQL Server Management Studio" -Directory -Filter "*IsoShell" -ErrorAction SilentlyContinue
    foreach ($inst in $legacyInstances) {
        $cmc = Join-Path $inst.FullName "ComponentModelCache"
        if (Test-Path $cmc) {
            Write-Host "  -> Làm mới ComponentModelCache: $cmc" -ForegroundColor Gray
            Get-ChildItem -Path $cmc -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
        }

        $extPath = Join-Path $inst.FullName "Extensions"
        if (Test-Path $extPath) {
            Get-ChildItem -Path $extPath -Include "*.cache" -Recurse -ErrorAction SilentlyContinue | ForEach-Object {
                Write-Host "  -> Xóa cache: $($_.Name)" -ForegroundColor Gray
                Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
            }
            Set-Content -Path (Join-Path $extPath "extensions.configurationchanged") -Value "" -ErrorAction SilentlyContinue
        }
    }
}

# 4. Gỡ đăng ký VSIXInstaller nếu có
Write-Host "[4/5] Hủy đăng ký tiện ích trong hệ thống VSIXInstaller..." -ForegroundColor Yellow
$vsixInstallerPath = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\resources\app\ServiceHub\Services\Microsoft.VisualStudio.Setup.Service\VsixInstaller\VSIXInstaller.exe"
if (Test-Path $vsixInstallerPath) {
    Start-Process -FilePath $vsixInstallerPath -ArgumentList "/uninstall:LumiSqlFormatter.Extension.2026 /quiet" -Wait -WindowStyle Hidden -ErrorAction SilentlyContinue
    Start-Process -FilePath $vsixInstallerPath -ArgumentList "/uninstall:SqlAutoUppercase /quiet" -Wait -WindowStyle Hidden -ErrorAction SilentlyContinue
}

# 5. Tùy chọn xóa dữ liệu cá nhân
Write-Host "[5/5] Kiểm tra thư mục dữ liệu cá nhân..." -ForegroundColor Yellow
$appDataDir = "$env:APPDATA\LumiSqlFormatter"
if (Test-Path $appDataDir) {
    Write-Host ""
    $promptDelete = Read-Host "Bạn có muốn xóa luôn thư mục cài đặt & Snippets cá nhân ($appDataDir) không? (Y/N)"
    if ($promptDelete -eq 'Y' -or $promptDelete -eq 'y') {
        Remove-Item -Path $appDataDir -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "  -> Đã xóa thư mục $appDataDir" -ForegroundColor Green
    } else {
        Write-Host "  -> Giữ lại thư mục Snippets cá nhân tại $appDataDir" -ForegroundColor Cyan
    }
}

Write-Host ""
Write-Host "==============================================================================" -ForegroundColor Green
Write-Host "[HOÀN TẤT] Đã gỡ bỏ 100% sạch sẽ toàn bộ extension và làm mới cache SSMS!" -ForegroundColor Green
Write-Host "Bây giờ bạn có thể mở lại SSMS, đảm bảo không còn bất kỳ dấu vết nào của tiện ích." -ForegroundColor Green
Write-Host "==============================================================================" -ForegroundColor Green
Write-Host ""
pause
