@echo off
:: ==============================================================================
::  Lumi SQL Formatter - Uninstall Launcher
:: ==============================================================================
chcp 65001 >nul
title Go cai dat hoan toan Lumi SQL Formatter

:: Kiem tra quyen Administrator
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo [THONG BAO] Dang yeu cau quyen Administrator de go cai dat...
    powershell -NoProfile -ExecutionPolicy Bypass -Command "Start-Process cmd -ArgumentList '/c \"\"%~f0\"\"' -Verb RunAs"
    exit /b
)

:: Khoi chay script PowerShell voi ho tro UTF-8 toan dien
powershell -NoProfile -ExecutionPolicy Bypass -Command "$OutputEncoding = [System.Text.Encoding]::UTF8; [Console]::OutputEncoding = [System.Text.Encoding]::UTF8; [Console]::InputEncoding = [System.Text.Encoding]::UTF8; & '%~dp0Uninstall.ps1'"
