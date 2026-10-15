# ⚡ HƯỚNG DẪN SỬ DỤNG & TÀI LIỆU KỸ THUẬT TOÀN DIỆN
# LUMI SQL FORMATTER & SMART SNIPPETS (PHONGNT)

> **Phiên bản:** 1.1.2  
> **Tác giả:** PHONGNT  
> **Dự án:** `LumiSqlFormatter.Extension.2026`  
> **Môi trường hỗ trợ:** SQL Server Management Studio (SSMS 18, 19, 20, 22+ x64)

---

## 📖 MỤC LỤC
1. [Giới thiệu tổng quan](#1-giới-thiệu-tổng-quan)
2. [Hướng dẫn cài đặt nhanh](#2-hướng-dẫn-cài-đặt-nhanh)
3. [Tự động viết hoa từ khóa (Real-time Auto-Capitalize)](#3-tự-động-viết-hoa-từ-khóa-real-time-auto-capitalize)
   - 3.1. Các chế độ định dạng Casing
   - 3.2. Cơ chế loại trừ đa dòng thông minh (Strict Multi-line Exclusions)
   - 3.3. Bảo vệ biến và bảng tạm (`@Year`, `#TempTable`)
   - 3.4. Danh mục hơn 510 từ khóa, kiểu dữ liệu và hàm T-SQL
4. [Quản lý & Sử dụng Snippets (Smart Snippet Manager)](#4-quản-lý--sử-dụng-snippets-smart-snippet-manager)
   - 4.1. Thư mục Snippets tùy biến & Định dạng JSON độc lập
   - 4.2. Gợi ý tự động khi gõ (IntelliSense Suggestion Popup)
   - 4.3. Tạo nhanh Snippet từ Editor (`Ctrl + Alt + S`)
   - 4.4. Trình quản lý chi tiết trong Tools -> Options (Lưới 3 dòng, Theme Sáng/Tối)
5. [Hướng dẫn gỡ cài đặt sạch sẽ (Clean Uninstall)](#5-hướng-dẫn-gỡ-cài-đặt-sạch-sẽ-clean-uninstall)
6. [Cấu trúc kỹ thuật của dự án](#6-cấu-trúc-kỹ-thuật-của-dự-án)
7. [Nhật ký cập nhật các phiên bản (Changelog)](#7-nhật-ký-cập-nhật-các-phiên-bản-changelog)

---

## 1. GIỚI THIỆU TỔNG QUAN

**Lumi SQL Formatter (PHONGNT)** là tiện ích mở rộng (extension) chuyên nghiệp dành cho SQL Server Management Studio (SSMS), mang lại trải nghiệm viết mã T-SQL hiện đại, chuẩn chỉ, lấy cảm hứng từ các tính năng cốt lõi của **Redgate SQL Prompt**:

- 🔠 **Tự động viết hoa từ khóa thời gian thực (Real-time Auto-Capitalize):** Tự động chuẩn hóa chữ hoa/chữ thường cho Keywords, Data Types và Built-in Functions ngay khi gõ phím cách (`Space`), `Enter`, `Tab`, phẩy (`,`), chấm phẩy (`;`), hoặc đóng ngoặc (`)`).
- 🛡 **Loại trừ ngữ cảnh đa dòng chính xác tuyệt đối (Strict Multi-line Exclusions):** Sử dụng bộ phân tích từ vựng (Lexical State Machine) quét trực tiếp trên Buffer Snapshot, nhận diện chính xác chuỗi ký tự nhiều dòng, chú thích lồng nhau, định danh trong ngoặc vuông và nháy kép.
- 🎯 **Bảo vệ biến T-SQL:** Biến cục bộ có tiền tố `@` (như `@Year`, `@Date`, `@Status`) và bảng tạm `#` (như `#Year`) không bao giờ bị viết hoa nhầm thành từ khóa SQL.
- ⚡ **Snippet Engine thông minh:** Gợi ý tức thì (suggestion popup), mở rộng mã mẫu bằng phím `Tab` hoặc `Enter`, hỗ trợ con trỏ `$CURSOR$`.
- 📁 **Thư mục Snippet tùy biến (Redgate-style):** Mỗi snippet được lưu thành file `{prefix}.json` riêng lẻ trong thư mục bạn chỉ định, dễ dàng đồng bộ qua Git/OneDrive.
- ⚙ **Tích hợp sâu vào SSMS:** Quản trị cấu hình toàn diện tại menu **Tools -> Options -> Lumi SQL Formatter** (hỗ trợ full-screen, lưới 3 dòng tối ưu không gian, giao diện Dark/Light mode).

---

## 2. HƯỚNG DẪN CÀI ĐẶT NHANH

Gói cài đặt VSIX được đóng gói chuẩn **VsixV2 (SSMS Isolated Shell)**, tương thích hoàn toàn với SSMS 18, 19, 20 và 22+.

### Cách 1: Cài đặt trực tiếp qua file .VSIX (Khuyên dùng)
1. Đóng hoàn toàn các cửa sổ SSMS đang chạy.
2. Mở thư mục chứa file cài đặt:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. **Nhấp đúp chuột** vào file `LumiSqlFormatter.vsix`.
4. Trình cài đặt `VSIX Installer` sẽ hiển thị: **Lumi SQL Formatter & Snippets (PHONGNT)** -> Nhấn **Install** (hoặc **Update** nếu cài đè).
5. Mở lại SSMS để bắt đầu sử dụng.

### Cách 2: Cài đặt tự động bằng 1-Click Script
Chạy file script đi kèm:
```cmd
d:\LumiSqlFormatter\Install-To-SSMS22.bat
```
Script sẽ tự động đóng SSMS, giải nén VSIX vào thư mục Extensions của SSMS 22, dọn sạch ComponentModelCache và khởi tạo cấu hình.

---

## 3. TỰ ĐỘNG VIẾT HOA TỪ KHÓA (REAL-TIME AUTO-CAPITALIZE)

### 3.1. Các chế độ định dạng Casing
Vào menu **Tools** -> **Options** -> **Lumi SQL Formatter** -> tab **Formatting & Casing**:

| Tùy chọn | Mô tả | Giá trị hỗ trợ |
| :--- | :--- | :--- |
| **Bật Tự động Viết hoa** | Bật/Tắt tính năng tự động viết hoa khi gõ phím | `Bật` / `Tắt` |
| **Keywords Casing** | Kiểu chữ cho từ khóa lệnh SQL (`SELECT`, `FROM`, `WHERE`, `JOIN`...) | `UPPERCASE`, `lowercase`, `PascalCase` |
| **Data Types Casing** | Kiểu chữ cho kiểu dữ liệu (`INT`, `VARCHAR`, `DATETIME2`...) | `UPPERCASE`, `lowercase`, `PascalCase` |
| **Functions Casing** | Kiểu chữ cho hàm có sẵn (`COUNT`, `ISNULL`, `GETDATE`, `STRING_AGG`...) | `UPPERCASE`, `lowercase`, `PascalCase` |

> [!TIP]
> **Các phím kích hoạt AutoUpperCase:**  
> Hệ thống sẽ kích hoạt kiểm tra và định dạng từ ngay trước con trỏ khi bạn gõ: **Phím cách (`Space`)**, **Phím Enter**, **Phím Tab**, **Dấu phẩy (`,`)**, **Dấu chấm phẩy (`;`)**, hoặc **Dấu đóng ngoặc (`)`)**.

---

### 3.2. Cơ chế loại trừ đa dòng thông minh (Strict Multi-line Exclusions)
Trong các phiên bản trước, việc chỉ phân tích dòng hiện tại dẫn đến lỗi: khi xuống dòng trong chuỗi ký tự nhiều dòng, từ khóa như `and` bị viết hoa sai thành `AND`.

Từ phiên bản 1.1.1+, Lumi SQL Formatter sử dụng **Lexical State Machine** chuyên biệt cho T-SQL quét trên toàn bộ Buffer Snapshot:
- **Chuỗi ký tự nhiều dòng (`'...'`, `N'...'`):** Nhận biết chuỗi mở kéo dài qua nhiều dòng (`\r\n`) cho đến khi gặp dấu nháy đóng hợp lệ.
- **Escape nháy kép (`''`):** Cặp nháy kép thoát bên trong chuỗi (ví dụ: `''''`) không làm ngắt chuỗi.
- **Chú thích nhiều dòng (`/* ... */`):** Theo dõi độ sâu chú thích, hỗ trợ cả chú thích lồng nhau (`/* /* nested */ */`).
- **Định danh trong ngoặc vuông (`[...]`) & nháy kép (`"..."`):** Không can thiệp vào tên cột/bảng đặt trong ngoặc.
- **Hiệu năng:** Quét $O(N)$ bằng C# thuần, xử lý tài liệu 500.000 ký tự chỉ mất **< 1 mili-giây** (hoàn toàn mượt mà, không giật lag).

---

### 3.3. Bảo vệ biến và bảng tạm (`@Year`, `#TempTable`)
- **Biến T-SQL (`@variable`):** Khi bạn khai báo hoặc sử dụng biến có tên trùng với từ khóa/hàm (ví dụ: `@Year`, `@Date`, `@Month`, `@Status`, `@Type`, `@Count`), hệ thống phát hiện tiền tố `@` và **giữ nguyên đúng chữ bạn gõ, không tự động in hoa**.
- **Hàm chuẩn:** Khi viết `YEAR(GetDate())`, do không có tiền tố `@`, chữ `YEAR` vẫn được tự động viết hoa chuẩn xác.
- **Bảng tạm (`#table`):** Tên bảng tạm bắt đầu bằng `#` (như `#Year`) cũng được bảo vệ an toàn.

---

### 3.4. Danh mục hơn 510 từ khóa, kiểu dữ liệu và hàm T-SQL
Hệ thống tích hợp sẵn từ điển **hơn 510 từ khóa độc nhất**, phân loại thành 3 nhóm:

1. **Cú pháp, Mệnh đề & Hints (280+ từ):**
   - **DQL / DML / DDL:** `SELECT`, `FROM`, `WHERE`, `GROUP`, `BY`, `HAVING`, `ORDER`, `ASC`, `DESC`, `INSERT`, `INTO`, `VALUES`, `UPDATE`, `SET`, `DELETE`, `MERGE`, `MATCHED`, `TARGET`, `SOURCE`, `USING`, `OUTPUT`, `DELETED`, `INSERTED`, `ACTION`, `TOP`, `DISTINCT`, `PERCENT`, `TIES`...
   - **Mệnh đề DDL nâng cao:** `CREATE`, `ALTER`, `DROP`, `TRUNCATE`, `TABLE`, `COLUMN`, `VIEW`, `PROCEDURE`, `PROC`, `FUNCTION`, `TRIGGER`, `INDEX`, `DATABASE`, `SCHEMA`, `SEQUENCE`, `SYNONYM`, `TYPE`...
   - **Joins & Set Operators:** `JOIN`, `INNER`, `LEFT`, `RIGHT`, `FULL`, `CROSS`, `OUTER`, `APPLY`, `ON`, `UNION`, `ALL`, `EXCEPT`, `INTERSECT`...
   - **Logic & Điều kiện:** `CASE`, `WHEN`, `THEN`, `ELSE`, `END`, `AND`, `OR`, `NOT`, `IN`, `IS`, `NULL`, `LIKE`, `BETWEEN`, `EXISTS`, `SOME`, `ANY`, `COLLATE`, `ESCAPE`...
   - **Phân trang & Windowing:** `OVER`, `PARTITION`, `ROWS`, `RANGE`, `UNBOUNDED`, `PRECEDING`, `FOLLOWING`, `CURRENT`, `ROW`, `OFFSET`, `FETCH`, `NEXT`, `ONLY`, `FIRST`, `LAST`...
   - **Pivot & Transform:** `PIVOT`, `UNPIVOT`, `TABLESAMPLE`...
   - **Giao dịch & Luồng:** `BEGIN`, `TRAN`, `TRANSACTION`, `COMMIT`, `ROLLBACK`, `SAVE`, `EXEC`, `EXECUTE`, `DECLARE`, `AS`, `WITH`, `IF`, `WHILE`, `BREAK`, `CONTINUE`, `RETURN`, `RETURNS`, `GOTO`, `WAITFOR`, `DELAY`...
   - **Xử lý lỗi:** `TRY`, `CATCH`, `THROW`, `RAISERROR`, `PRINT`...
   - **Ràng buộc & Chỉ mục:** `PRIMARY`, `FOREIGN`, `REFERENCES`, `CONSTRAINT`, `CHECK`, `UNIQUE`, `CLUSTERED`, `NONCLUSTERED`, `COLUMNSTORE`, `CASCADE`, `RESTRICT`, `NOCHECK`, `INCLUDE`, `FILLFACTOR`, `PAD_INDEX`, `DATA_COMPRESSION`, `ONLINE`, `OFFLINE`, `PERSISTED`, `SPARSE`, `IDENTITY`, `IDENTITY_INSERT`...
   - **Lock & Query Hints:** `OPTION`, `RECOMPILE`, `MAXRECURSION`, `OPTIMIZE`, `FAST`, `FORCE`, `LOOP`, `HASH`, `NOLOCK`, `READUNCOMMITTED`, `READCOMMITTED`, `REPEATABLEREAD`, `SERIALIZABLE`, `READPAST`, `XLOCK`, `UPDLOCK`, `ROWLOCK`, `PAGLOCK`, `TABLOCK`, `TABLOCKX`, `HOLDLOCK`, `NOWAIT`...
   - **Thiết lập phiên làm việc (SET):** `NOCOUNT`, `XACT_ABORT`, `ANSI_NULLS`, `QUOTED_IDENTIFIER`, `ARITHABORT`, `NUMERIC_ROUNDABORT`, `ANSI_PADDING`, `ANSI_WARNINGS`, `ISOLATION`, `LEVEL`, `SNAPSHOT`, `STATISTICS`, `IO`, `PROFILE`, `OFF`, `ON`, `GO`, `USE`...
   - **Cursor, XML, JSON & Full-Text:** `CURSOR`, `OPEN`, `CLOSE`, `DEALLOCATE`, `PATH`, `AUTO`, `EXPLICIT`, `RAW`, `ELEMENTS`, `ROOT`, `OPENROWSET`, `OPENDATASOURCE`, `OPENQUERY`, `OPENXML`, `OPENJSON`, `CONTAINS`, `FREETEXT`, `READONLY`, `OUT`...

2. **Kiểu dữ liệu T-SQL (37 kiểu):**
   - `INT`, `INTEGER`, `BIGINT`, `SMALLINT`, `TINYINT`, `BIT`
   - `DECIMAL`, `NUMERIC`, `MONEY`, `SMALLMONEY`, `FLOAT`, `REAL`
   - `DATE`, `DATETIME`, `DATETIME2`, `DATETIMEOFFSET`, `SMALLDATETIME`, `TIME`
   - `CHAR`, `CHARACTER`, `VARCHAR`, `NCHAR`, `NVARCHAR`, `TEXT`, `NTEXT`
   - `BINARY`, `VARBINARY`, `IMAGE`
   - `UNIQUEIDENTIFIER`, `XML`, `SQL_VARIANT`, `GEOMETRY`, `GEOGRAPHY`, `HIERARCHYID`, `ROWVERSION`, `TIMESTAMP`, `SYSNAME`, `JSON`, `VECTOR`

3. **Hàm tích hợp & Hệ thống (180+ hàm):**
   - **Tập hợp & Cửa sổ:** `COUNT`, `COUNT_BIG`, `SUM`, `AVG`, `MIN`, `MAX`, `STRING_AGG`, `APPROX_COUNT_DISTINCT`, `CHECKSUM_AGG`, `STDEV`, `VAR`, `GROUPING`, `ROW_NUMBER`, `RANK`, `DENSE_RANK`, `NTILE`, `LEAD`, `LAG`, `FIRST_VALUE`, `LAST_VALUE`, `PERCENT_RANK`, `CUME_DIST`...
   - **Xử lý chuỗi:** `ASCII`, `CHAR`, `CHARINDEX`, `CONCAT`, `CONCAT_WS`, `DIFFERENCE`, `FORMAT`, `LEFT`, `LEN`, `LOWER`, `LTRIM`, `NCHAR`, `PATINDEX`, `QUOTENAME`, `REPLACE`, `REPLICATE`, `REVERSE`, `RIGHT`, `RTRIM`, `SOUNDEX`, `SPACE`, `STR`, `STRING_ESCAPE`, `STRING_SPLIT`, `STUFF`, `SUBSTRING`, `TRANSLATE`, `TRIM`, `UNICODE`, `UPPER`, `DATALENGTH`...
   - **Ngày giờ:** `GETDATE`, `GETUTCDATE`, `SYSDATETIME`, `SYSUTCDATETIME`, `SYSDATETIMEOFFSET`, `DATEADD`, `DATEDIFF`, `DATEDIFF_BIG`, `DATEPART`, `DATENAME`, `DATEFROMPARTS`, `EOMONTH`, `ISDATE`, `YEAR`, `MONTH`, `DAY`...
   - **Chuyển đổi & Điều kiện:** `CAST`, `CONVERT`, `PARSE`, `TRY_CAST`, `TRY_CONVERT`, `TRY_PARSE`, `ISNULL`, `COALESCE`, `NULLIF`, `IIF`, `CHOOSE`, `GREATEST`, `LEAST`...
   - **Toán học:** `ABS`, `ACOS`, `ASIN`, `ATAN`, `CEILING`, `COS`, `EXP`, `FLOOR`, `LOG`, `LOG10`, `PI`, `POWER`, `RAND`, `ROUND`, `SIGN`, `SIN`, `SQRT`, `SQUARE`, `TAN`...
   - **Metadata & Đối tượng:** `SCOPE_IDENTITY`, `IDENT_CURRENT`, `OBJECT_ID`, `OBJECT_NAME`, `OBJECT_SCHEMA_NAME`, `OBJECT_DEFINITION`, `OBJECTPROPERTY`, `COLUMNPROPERTY`, `COL_LENGTH`, `INDEXPROPERTY`, `DATABASEPROPERTYEX`, `DB_ID`, `DB_NAME`, `SCHEMA_ID`, `SCHEMA_NAME`, `TYPE_ID`, `SERVERPROPERTY`, `USER_ID`, `USER_NAME`, `APP_NAME`, `NEWID`, `NEWSEQUENTIALID`, `ERROR_MESSAGE`, `ROWCOUNT`, `TRANCOUNT`, `SPID`...

---

## 4. QUẢN LÝ & SỬ DỤNG SNIPPETS (SMART SNIPPET MANAGER)

### 4.1. Thư mục Snippets tùy biến & Định dạng JSON độc lập
Bạn có thể trỏ thư mục lưu trữ snippet đến bất kỳ đâu (ví dụ: `D:\Work\MySnippets` hoặc thư mục đồng bộ OneDrive/Git).

Mỗi snippet được lưu độc lập dưới dạng một file `{prefix}.json`:
```json
{
  "id": "1dd28f3b-f3ef-4868-b3db-b502a682f22f",
  "prefix": "adrop",
  "description": "Drop toàn bộ bảng tạm khi debug",
  "body": "-- Drop bảng tạm khi debug\ndeclare @zsql nvarchar(max); select @zsql = isnull(@zsql + ';', '') + 'drop table ' + quotename(name) from tempdb..sysobjects where name like '#%' and OBJECT_ID('tempdb..' + name) is not null; exec (@zsql)"
}
```

- **`prefix`**: Từ viết tắt để kích hoạt snippet (ví dụ: `ssf`, `adrop`, `ct`, `ii`).
- **`description`**: Mô tả chức năng hiển thị trong suggestion popup.
- **`body`**: Đoạn mã mẫu. Hỗ trợ biến `$CURSOR$` xác định vị trí con trỏ chuột dừng lại sau khi bung mã.

---

### 4.2. Gợi ý tự động khi gõ (IntelliSense Suggestion Popup)
1. Trong Query Editor của SSMS, gõ các ký tự đầu của prefix (ví dụ: gõ `a` hoặc `s`).
2. Một popup gợi ý nổi lên ngay dưới con trỏ:
   - Cột trái: Danh sách các snippet phù hợp kèm badge shortcut nổi bật.
   - Cột phải: Khung xem trước code template hoàn chỉnh.
   - Dùng phím **`Up` / `Down`** để chọn snippet.
   - Nhấn **`Tab` hoặc `Enter`** để chèn toàn bộ đoạn code vào editor.
   - Nhấn **`Esc`** để đóng popup gợi ý.

---

### 4.3. Tạo nhanh Snippet từ Editor (`Ctrl + Alt + S`)
1. Trong Query Editor, bôi đen đoạn mã SQL bạn muốn lưu làm template.
2. Nhấn tổ hợp phím **`Ctrl + Alt + S`**.
3. Cửa sổ **Quick Create Snippet** sẽ mở ra với đoạn code đã được điền sẵn:
   - Nhập **Prefix** (ví dụ: `myquery`).
   - Nhập **Mô tả**.
   - Bấm nút **📍 Chèn $CURSOR$** tại vị trí con trỏ mong muốn.
   - Nhấn **`Enter`** hoặc bấm **💾 Lưu Snippet**.

---

### 4.4. Trình quản lý chi tiết trong Tools -> Options
Vào menu **Tools** -> **Options** -> **Lumi SQL Formatter** -> tab **Snippet Manager**:

- **Khắc phục triệt để lỗi nhập liệu:** Sử dụng `UIElementDialogPage` cùng bộ hook `WM_GETDLGCODE` chuyên sâu, cho phép gõ phím trực tiếp vào các ô Prefix, Mô tả và Body mà không bị chặn bởi Win32 Modal Dialog của SSMS.
- **Lưới hiển thị đúng 3 dòng (Compact DataGrid):** Danh sách snippets được giới hạn hiển thị 3 dòng (`Height="108"`), tự động xuất hiện thanh cuộn bên trong khi có nhiều snippet, dành trọn vẹn không gian cho khung soạn thảo code chi tiết bên dưới.
- **Tùy chọn Theme Sáng / Tối (Theme Mode):** Cho phép chọn chế độ **Light Mode** hoặc **Dark Mode** với độ tương phản cao, chữ sắc nét, dễ nhìn.
- **Nút "Làm mới" (Refresh):** Bấm nút **🔄 Làm mới** để quét lại toàn bộ thư mục snippet nếu bạn vừa pull code từ Git hoặc chỉnh sửa file từ bên ngoài.
- **Mặc định không cài đặt snippet mẫu:** Hệ thống để danh sách trống để bạn hoàn toàn chủ động xây dựng bộ snippet cá nhân hoặc trỏ đến kho snippet của riêng mình.

---

## 5. HƯỚNG DẪN GỠ CÀI ĐẶT SẠCH SẼ (CLEAN UNINSTALL)

Nếu cần gỡ bỏ extension hoàn toàn khỏi SSMS:
1. Đóng toàn bộ cửa sổ SSMS.
2. Chạy file script:
   ```cmd
   d:\LumiSqlFormatter\Uninstall.bat
   ```
   *(Hoặc chạy `Uninstall.ps1` bằng PowerShell Admin)*.
3. Script sẽ:
   - Gỡ bỏ extension khỏi thư mục Extensions của SSMS.
   - Xóa bộ đệm MEF ComponentModelCache.
   - Chạy `SSMS.exe /updateconfiguration` để làm sạch thanh menu và registry.

---

## 6. CẤU TRÚC KỸ THUẬT CỦA DỰ ÁN

```text
d:\LumiSqlFormatter\
├── LumiSqlFormatter.sln                        # Visual Studio Solution
├── Install-To-SSMS22.bat                       # Script cài đặt tự động vào SSMS
├── Uninstall.bat / Uninstall.ps1               # Script gỡ cài đặt sạch 100%
├── HUONG_DAN_SU_DUNG.md                        # Tài liệu hướng dẫn sử dụng toàn diện
├── README.md                                   # Giới thiệu tổng quan dự án
└── LumiSqlFormatter\
    ├── LumiSqlFormatter.csproj                 # Cấu hình VSSDK, SSMS References
    ├── source.extension.vsixmanifest           # VSIX Manifest (v1.1.2 - PHONGNT)
    ├── ExtensionPackage.cs                     # AsyncPackage đăng ký Menu, ToolWindow, OptionsPage
    ├── ExtensionPackage.vsct                   # Khai báo Top-Level Menu "Lumi SQL"
    ├── PostProcessVsix.ps1                     # Script đóng gói chuẩn SSMS VsixV2
    ├── Models/
    │   ├── CasingStyle.cs                      # Enum: Uppercase, Lowercase, PascalCase
    │   ├── FormatterSettings.cs                # Model cài đặt Casing & Snippets
    │   └── SnippetItem.cs                      # Model Snippet (Id, Prefix, Description, Body)
    ├── Services/
    │   ├── SettingsService.cs                  # Quản lý đọc/ghi JSON tại folder tùy biến
    │   ├── SqlTokenParsingService.cs           # Lexical State Scanner & 510+ tokens dictionary
    │   └── SnippetService.cs                   # Mở rộng template, tính toán $CURSOR$, prefix match
    ├── Editor/
    │   ├── TextViewCreationListener.cs         # MEF Component gắn KeyCommandFilter vào Query Editor
    │   └── KeyCommandFilter.cs                 # IOleCommandTarget bắt phím Space, Enter, Tab, ), v.v.
    ├── UI/
    │   ├── SettingsToolWindow.cs               # ToolWindow nhúng SettingsView
    │   ├── OptionsPage.cs                      # UIElementDialogPage trong Tools -> Options
    │   ├── Views/
    │   │   ├── SettingsView.xaml (.cs)         # Giao diện chính (Lưới 3 dòng, Theme Dark/Light)
    │   │   ├── QuickSnippetWindow.xaml (.cs)   # Modal tạo nhanh snippet từ editor (Ctrl+Alt+S)
    │   │   └── SnippetSuggestionPopup.xaml     # Popup gợi ý snippet cạnh con trỏ
    │   └── ViewModels/
    │       ├── ViewModelBase.cs / RelayCommand.cs
    │       ├── QuickSnippetViewModel.cs
    │       └── SettingsViewModel.cs            # Binding logic, CRUD snippets, Theme logic
    └── Properties/
        └── AssemblyInfo.cs                     # Version 1.1.2.0
```

---

## 7. NHẬT KÝ CẬP NHẬT CÁC PHIÊN BẢN (CHANGELOG)

### 📌 Phiên bản 1.1.2 (Bản hiện tại)
- **Bổ sung từ khóa `COLUMN`:** Đã thêm vào từ điển cú pháp DDL (`ALTER TABLE ... ADD COLUMN ...`).
- **Xử lý triệt để biến T-SQL (`@Year`):**
  - Bảo vệ các biến có tiền tố `@` (như `@Year`, `@Date`, `@Status`, `@Month`, `@Type`...) không bị tự động viết hoa nhầm thành từ khóa SQL.
  - Bảo vệ tên bảng tạm dạng `#Year`.
  - Giữ nguyên tính năng viết hoa cho hàm chuẩn `YEAR(...)`.
- **Mở rộng bộ từ điển lên hơn 510 từ khóa & hàm:** Bao gồm đầy đủ DQL, DML, DDL, Windowing, Pivot, Transaction, Query Hints, Lock Hints, SET Options, 37 kiểu dữ liệu và 180+ hàm tích hợp.
- **Bổ sung dấu đóng ngoặc `)` vào danh sách phím kích hoạt AutoUpperCase:** Gõ `CAST(col AS int)` rồi gõ `)` sẽ tự động chuyển `int` thành `INT`.
- **Cập nhật hiển thị:** Bổ sung nhãn **`(PHONGNT)`** vào `DisplayName` gói VSIX, tiêu đề cửa sổ và giao diện cấu hình.

---

### 📌 Phiên bản 1.1.1
- **Khắc phục lỗi AutoUpperCase trong chuỗi ký tự nhiều dòng (Multi-line String):**
  - Thay thế cơ chế kiểm tra từng dòng đơn lẻ bằng bộ quét **Lexical State Machine** đa dòng quét trên Buffer Snapshot.
  - Tự động nhận diện chuỗi mở kéo dài qua các ký tự xuống dòng (`\r\n`) cho đến khi gặp dấu nháy đóng hợp lệ.
  - Xử lý chính xác cặp nháy đơn thoát (`''`) bên trong chuỗi.
  - Hỗ trợ đầy đủ khối chú thích nhiều dòng lồng nhau (`/* ... */`).
  - Tốc độ quét cực cao: < 1ms cho tài liệu 500.000 ký tự.

---

### 📌 Phiên bản 1.1.0
- **Khắc phục dứt điểm lỗi không nhập liệu trong hộp thoại Options:**
  - Chuyển `OptionsPage` sang kế thừa `UIElementDialogPage` tích hợp chuẩn `IKeyboardInputSink`.
  - Xử lý thông điệp `WM_GETDLGCODE` chuyên sâu, khôi phục `TranslateMessage` để WPF TextBoxes nhận thông điệp `WM_CHAR`.
  - Tự động kích hoạt Win32 focus và bôi đen ô `Prefix` khi bấm Thêm mới.
- **Tối ưu hóa giao diện Snippet:**
  - Thu gọn lưới danh sách Snippet xuống đúng 3 dòng (`Height="108"`), nhường toàn bộ không gian cho khung soạn thảo chi tiết.
  - Bổ sung tùy chọn giao diện **Light Mode / Dark Mode** với màu chữ tương phản cao, sắc nét.
  - Đổi tên nút thành **"Làm mới" (Refresh)** để tải lại thư mục snippet.
  - Xóa toàn bộ snippet mặc định, cho phép người dùng tự quản lý danh mục riêng.

---

### 📌 Phiên bản 1.0.0
- Khởi tạo dự án Lumi SQL Formatter cho SSMS 22 (x64).
- Tự động viết hoa thời gian thực các từ khóa cơ bản.
- Hỗ trợ lưu trữ snippet theo định dạng file JSON riêng lẻ (Redgate style).
- Tích hợp popup gợi ý snippet và tạo nhanh qua `Ctrl + Alt + S`.
