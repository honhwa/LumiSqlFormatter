# Lumi SQL Formatter & Smart Snippets (PHONGNT)

[![SSMS Support](https://img.shields.io/badge/SSMS-18%20%7C%2019%20%7C%2020%20%7C%2022+-007ACC.svg)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
[![VSIX Version](https://img.shields.io/badge/version-1.1.3-2D7D9A.svg)](d:/LumiSqlFormatter/LumiSqlFormatter/bin/Release/LumiSqlFormatter.vsix)
[![Author](https://img.shields.io/badge/author-PHONGNT-107C41.svg)](https://github.com)

**[English](#english) | [Tiếng Việt](#tiếng-việt)**

---

<a name="english"></a>
## English

An enterprise-grade extension for **SQL Server Management Studio (SSMS 18, 19, 20, 22+)** built on the 64-bit Visual Studio Isolated Shell, inspired by the core productivity features of **Redgate SQL Prompt**:

### Key Features

1. **Real-Time Auto-Capitalization:**
   - Automatically formats keywords, data types, and built-in functions as you type upon pressing trigger keys (`Space`, `Enter`, `Tab`, `,`, `;`, `)`).
   - Configurable casing styles: `UPPERCASE`, `lowercase`, or `PascalCase`.

2. **Comprehensive Dictionary (510+ T-SQL Keywords & Functions):**
   - Full T-SQL DQL, DML, and DDL syntax coverage (including `COLUMN`).
   - Window Functions, PIVOT/UNPIVOT, Transactions, Query & Table Hints, and SET Options.
   - 37 Data Types and 180+ T-SQL Built-in Functions.

3. **Strict Multi-Line Lexical Exclusions:**
   - Built on a snapshot-based Lexical State Machine (Microsoft ScriptDom Lexer integration).
   - Safely ignores: multi-line string literals (`'...'`, `N'...'`), escaped quotes (`''`), nested comments (`/* ... */`, `--`), and bracketed or double-quoted identifiers (`[...]`, `"..."`).

4. **T-SQL Variable & Temporary Table Protection:**
   - User-defined local variables (`@Year`, `@Date`, `@Status`, etc.) and temporary tables (`#Year`, `#Temp`) are strictly protected from accidental capitalization.

5. **Smart Snippet Manager:**
   - **Instant Expansion:** Expand code templates immediately using `Tab` or `Enter` after typing the snippet prefix (e.g., `ssf`, `adrop`, `ct`, `ii`...). Supports final cursor positioning via `$CURSOR$`.
   - **Custom Snippet Directory:** Point your snippets library to any custom directory (OneDrive, Git repository, network share). Saved as individual `{prefix}.json` files (Redgate style) for seamless version control and team sharing.
   - **Quick Snippet Creation (`Ctrl+Alt+S`):** Highlight any query in the Query Editor and press `Ctrl+Alt+S` to open an instant snippet creator modal.
   - **IntelliSense Suggestion Popup:** Floating suggestion box appears next to the cursor showing matching snippets with live template preview.

6. **Native Enterprise Options Page:**
   - Deeply integrated into SSMS at **Tools -> Options -> Lumi SQL Formatter & Snippets (PHONGNT)**.
   - Clean, professional styling matching Visual Studio / SSMS design standards (no distracting AI motifs).
   - Responsive Dark / Light theme support for live syntax preview and snippet editors.
   - High-DPI scaling compatible layout with compact, resizable 3-row DataGrid.

---

### Project Structure

```text
d:\LumiSqlFormatter\
├── LumiSqlFormatter.sln                        # Visual Studio Solution
├── HUONG_DAN_SU_DUNG.md                        # Comprehensive user guide (Vietnamese)
├── README.md                                   # Project overview (EN & VI)
└── LumiSqlFormatter\
    ├── LumiSqlFormatter.csproj                 # VSSDK & SSMS project file
    ├── source.extension.vsixmanifest           # VSIX Manifest (v1.1.3 - PHONGNT)
    ├── ExtensionPackage.cs                     # AsyncPackage registering Top-Menu, ToolWindow, OptionsPage
    ├── ExtensionPackage.vsct                   # Top-Level Menu "Lumi SQL" definitions
    ├── PostProcessVsix.ps1                     # SSMS VsixV2 packaging automation script
    ├── Models/                                 # CasingStyle, FormatterSettings, SnippetItem models
    ├── Services/                               # SettingsService, SqlTokenParsingService, SnippetService
    ├── Editor/                                 # TextViewCreationListener, KeyCommandFilter
    ├── UI/                                     # SettingsToolWindow, OptionsPage, Views, ViewModels
    └── Properties/                             # AssemblyInfo.cs (v1.1.3.0)
```

---

### Installation via .VSIX

The extension is distributed as a standalone **`.vsix`** installation package:

1. Close all running instances of SQL Server Management Studio.
2. Locate the `.vsix` file at:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. Double-click the file to open **VSIX Installer** -> Click **Install** (or **Modify/Update**).
4. Restart SSMS to start using the extension.

> [!NOTE]
> For in-depth technical documentation, keyword catalogs, and version changelogs, please refer to [HUONG_DAN_SU_DUNG.md](file:///d:/LumiSqlFormatter/HUONG_DAN_SU_DUNG.md).

---

<a name="tiếng-việt"></a>
## Tiếng Việt

Extension chuyên nghiệp cho **SQL Server Management Studio (SSMS 18, 19, 20, 22+)** (Visual Studio Shell x64), được thiết kế lấy cảm hứng từ các tính năng cốt lõi của **Redgate SQL Prompt**:

### Tính Năng Nổi Bật

1. **Auto-Capitalization thời gian thực:** Tự động chuẩn hóa từ khóa khi kết thúc từ (`Space`, `Enter`, `Tab`, `,`, `;`, `)`) theo các chuẩn `UPPERCASE`, `lowercase`, hoặc `PascalCase`.
2. **Bộ từ điển 510+ Từ Khóa & Hàm T-SQL:** Bao gồm đầy đủ câu lệnh DQL/DML/DDL (kèm `COLUMN`), Window Functions, Pivot, Transaction, Hints, SET Options, 37 Data Types và 180+ Built-in Functions.
3. **Strict Multi-line Exclusions:** Bộ phân tích từ vựng (Lexical State Machine) trên Buffer Snapshot loại trừ an toàn: chuỗi ký tự nhiều dòng (`'...'`, `N'...'`), escape nháy kép (`''`), comment lồng nhau (`/* ... */`), và định danh trong ngoặc vuông (`[...]`, `"..."`).
4. **Bảo vệ biến T-SQL & Bảng tạm:** Các biến cục bộ `@Year`, `@Date`, `@Status`... và bảng tạm `#Year` không bao giờ bị viết hoa nhầm thành từ khóa SQL.
5. **Smart Snippet Manager:**
   - Kích hoạt nhanh template bằng phím `Tab` hoặc `Enter` khi gõ prefix (ví dụ: `ssf`, `adrop`, `ct`, `ii`...). Hỗ trợ vị trí đặt con trỏ cuối cùng `$CURSOR$`.
   - **Tùy chỉnh thư mục Snippet (Custom Directory):** Cho phép trỏ tới bất kỳ thư mục nào (OneDrive, Git repo, Shared drive...). Lưu dưới dạng từng file `{prefix}.json` độc lập (Redgate style).
   - **Khung nhập nhanh Snippet từ Editor (`Ctrl+Alt+S`):** Bôi đen đoạn code bất kỳ trong Query Editor, nhấn `Ctrl+Alt+S` để mở modal tạo nhanh template ngay lập tức.
   - **Suggestion Popup khi gõ:** Popup IntelliSense nổi lên ngay cạnh con trỏ soạn thảo hiển thị danh sách gợi ý và xem trước code template.
6. **Giao diện quản trị chuẩn mực:** Tích hợp trực tiếp vào **Tools -> Options -> Lumi SQL Formatter**, giao diện thiết kế theo phong cách native enterprise của Visual Studio / SSMS, hỗ trợ lưới 3 dòng thu gọn tối ưu không gian, giao diện Dark Mode / Light Mode sắc nét và nút Làm mới.

---

### Hướng Dẫn Cài Đặt Qua File .VSIX

Gói extension được phân phối trực tiếp dưới dạng file cài đặt chuẩn **`.vsix`**:

1. Đóng hoàn toàn các cửa sổ SSMS đang chạy.
2. Mở file `.vsix` tại đường dẫn:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. Nhấp đúp chuột vào file để khởi chạy **VSIX Installer** -> Chọn **Install** (hoặc **Modify/Update**).
4. Khởi động lại SSMS để bắt đầu sử dụng.

> [!NOTE]
> Để xem chi tiết hướng dẫn sử dụng tất cả tính năng, danh mục 510+ từ khóa, và nhật ký các phiên bản, vui lòng xem tại: [HUONG_DAN_SU_DUNG.md](file:///d:/LumiSqlFormatter/HUONG_DAN_SU_DUNG.md).
