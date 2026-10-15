# ⚡ HƯỚNG DẪN SỬ DỤNG & CÀI ĐẶT LUMI SQL FORMATTER

> **Phiên bản:** 1.0.0 (Hỗ trợ SQL Server Management Studio SSMS 18, 19, 20, 22+)  
> **Dự án:** `LumiSqlFormatter.Extension.2026`

---

## 📖 MỤC LỤC
1. [Giới thiệu tổng quan](#1-giới-thiệu-tổng-quan)
2. [Hướng dẫn cài đặt nhanh qua file .vsix](#2-hướng-dẫn-cài-đặt-nhanh-qua-file-vsix)
3. [Cấu hình Casing & Tự động viết hoa](#3-cấu-hình-casing--tự-động-viết-hoa)
4. [Quản lý & Sử dụng Snippets (Chuẩn JSON mới)](#4-quản-lý--sử-dụng-snippets-chuẩn-json-mới)
5. [Hướng dẫn gỡ cài đặt sạch 100% (Clean Uninstall)](#5-hướng-dẫn-gỡ-cài-đặt-sạch-100-clean-uninstall)
6. [Cấu trúc kỹ thuật của dự án](#6-cấu-trúc-kỹ-thuật-của-dự-án)

---

## 1. GIỚI THIỆU TỔNG QUAN

**Lumi SQL Formatter** là tiện ích mở rộng (extension) chuyên nghiệp dành cho SQL Server Management Studio (SSMS), mang lại trải nghiệm viết mã T-SQL hiện đại, chuẩn chỉ và tối ưu tốc độ:

- 🔠 **Tự động viết hoa từ khóa thời gian thực (Real-time Auto-Capitalize):** Tự động chuẩn hóa chữ hoa/chữ thường cho Keywords, Data Types và Built-in Functions ngay khi người dùng gõ phím cách (Space), Enter, Tab, phẩy (`,`), chấm phẩy (`;`).
- 🛡 **Bỏ qua an toàn (Strict Exclusions):** Tuyệt đối không can thiệp vào chuỗi ký tự (`'...'`, `N'...'`), khối comment (`--`, `/* ... */`) hay các định danh có ngoặc (`[Table]`, `"Column"`).
- ⚡ **Snippet Engine thông minh:** Gợi ý tức thì (suggestion popup), mở rộng mã mẫu bằng phím `Tab` hoặc `Enter`, hỗ trợ con trỏ `$CURSOR$`.
- 📁 **Thư mục Snippet tùy biến (Redgate-style):** Mỗi snippet được lưu thành file `{prefix}.json` độc lập trong thư mục bạn chọn, dễ dàng quản lý, chia sẻ và backup qua Git.
- ⚙ **Tích hợp sâu vào SSMS:** Quản trị cấu hình toàn diện tại menu chuẩn **Tools -> Options -> Lumi SQL Formatter**.

---

## 2. HƯỚNG DẪN CÀI ĐẶT NHANH QUA FILE .VSIX

Gói cài đặt VSIX đã được đóng gói chuẩn **VsixV2 (SSMS Isolated Shell)**, loại bỏ toàn bộ các ràng buộc gây lỗi dependency của Visual Studio.

### Các bước cài đặt:
1. Đảm bảo bạn đã đóng tất cả cửa sổ SSMS.
2. Tìm đến file `.vsix` đã build sẵn tại:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. **Nhấp đúp chuột** vào file `LumiSqlFormatter.vsix`.
4. Trình cài đặt `VSIX Installer` sẽ tự động phát hiện phiên bản SSMS (ví dụ: *SQL Server Management Studio 22*) -> Nhấn **Install**.
5. Mở lại SSMS để bắt đầu sử dụng.

---

## 3. CẤU HÌNH CASING & TỰ ĐỘNG VIẾT HOA

Vào menu **Tools** -> **Options** -> chọn mục **Lumi SQL Formatter**:

| Tùy chọn | Mô tả | Giá trị hỗ trợ |
| :--- | :--- | :--- |
| **Bật Tự động Viết hoa** | Bật/Tắt tính năng tự động chuẩn hóa từ khóa | `Bật` / `Tắt` |
| **Keywords Casing** | Kiểu chữ cho từ khóa lệnh SQL (`SELECT`, `FROM`, `WHERE`,...) | `UPPERCASE`, `lowercase`, `PascalCase` |
| **Data Types Casing** | Kiểu chữ cho kiểu dữ liệu (`INT`, `VARCHAR`, `DATETIME2`,...) | `UPPERCASE`, `lowercase`, `PascalCase` |
| **Functions Casing** | Kiểu chữ cho hàm có sẵn (`COUNT`, `ISNULL`, `GETDATE`,...) | `UPPERCASE`, `lowercase`, `PascalCase` |

Khung **Live Real-time Preview** ở bên phải sẽ cập nhật trực quan ngay lập tức kết quả định dạng khi bạn thay đổi các tùy chọn trên.

---

## 4. QUẢN LÝ & SỬ DỤNG SNIPPETS (CHUẨN JSON MỚI)

### 4.1. Cấu trúc chuẩn của Snippet JSON
Toàn bộ hệ thống quản lý snippet (tải file, lưu file, import, export) tuân theo 100% cấu trúc JSON sau:

```json
{
  "id": "1dd28f3b-f3ef-4868-b3db-b502a682f22f",
  "prefix": "adrop",
  "description": "Drop bảng tạm khi debugs",
  "body": "--Lệnh drop bảng tạm khi debug\ndeclare @zsql nvarchar(max); select @zsql = isnull(@zsql + ';', '') + 'drop table ' + quotename(name) from tempdb..sysobjects where name like '#%' and OBJECT_ID('tempdb..' + name) is not null; exec (@zsql)"
}
```

- **`id`**: Chuỗi GUID định danh duy nhất (tự sinh tự động nếu để trống).
- **`prefix`**: Từ khóa viết tắt để kích hoạt snippet trong trình soạn thảo (ví dụ: `adrop`, `ssf`, `ct`, `ii`).
- **`description`**: Mô tả chức năng của snippet.
- **`body`**: Nội dung đoạn mã mẫu. Bạn có thể chèn `$CURSOR$` để xác định vị trí con trỏ chuột dừng lại sau khi snippet được bung ra.

---

### 4.2. Cách sử dụng Snippet trong trình soạn thảo SSMS
1. Mở cửa sổ truy vấn mới (New Query) trong SSMS.
2. Gõ `prefix` của snippet, ví dụ:
   ```sql
   adrop
   ```
3. Một bảng danh sách gợi ý (**Snippet Suggestion Popup**) sẽ xuất hiện với thông tin mô tả và khung xem trước code.
4. Nhấn phím **Tab** hoặc **Enter**: Đoạn mã drop bảng tạm hoàn chỉnh sẽ được chèn vào, con trỏ tự động nhảy đến vị trí làm việc.

---

### 4.3. Tạo nhanh Snippet từ vùng chọn (Ctrl + Alt + S)
1. Trong cửa sổ Query của SSMS, bôi đen đoạn SQL bạn muốn biến thành snippet.
2. Nhấn tổ hợp phím **`Ctrl + Alt + S`**.
3. Cửa sổ **Quick Create Snippet** sẽ mở ra:
   - Nhập **Prefix (Phím tắt)** (ví dụ: `myq`).
   - Nhập **Mô tả (Description)**.
   - Bấm nút **📍 Chèn $CURSOR$** tại vị trí bạn muốn đặt con trỏ sau khi chèn.
   - Nhấn **Enter** hoặc bấm **💾 Lưu Snippet**.
4. Snippet mới sẽ lập tức có hiệu lực và được lưu thành file `myq.json` trong thư mục snippets của bạn.

---

### 4.4. Tùy chỉnh thư mục Snippet & Nút Refresh
Trong tab **Snippet Manager** (Tools -> Options -> Lumi SQL Formatter):
- **Khởi đầu hoàn toàn sạch sẽ:** Tiện ích không chèn bất kỳ snippet mặc định nào để bạn hoàn toàn chủ động trỏ tới kho snippet của riêng mình.
- **Browse...**: Cho phép bạn chọn một thư mục bất kỳ (ví dụ: `D:\MySqlSnippets\` hoặc thư mục đồng bộ Google Drive / OneDrive / Git).
- **Mở thư mục**: Mở trực tiếp thư mục snippets bằng Windows Explorer.
- **🔄 Refresh**: Cơ chế đọc lại toàn bộ thư mục snippet đang được trỏ tới. Bất cứ khi nào bạn copy, thêm bớt file `.json` bên ngoài hoặc cập nhật từ Git, chỉ cần bấm **Refresh** là toàn bộ snippets lập tức được nạp lại vào SSMS.
- **📥 Import JSON / 📤 Export JSON**: Nhập hoặc xuất toàn bộ danh sách snippet dưới dạng file JSON.

---

### 4.5. Trải nghiệm Full Screen trong Tools -> Options
Khi bạn mở **Tools** -> **Options** và chọn **Lumi SQL Formatter -> General**:
- Cửa sổ Options sẽ **tự động phóng to toàn màn hình (Full Screen)**.
- Giao diện bảng danh sách Snippets, khung soạn thảo code và Live Preview mở rộng tối đa, giúp việc quản lý và viết code template SQL thoải mái, chuyên nghiệp như một ứng dụng độc lập.

---

### 4.6. Tùy chọn Giao diện Snippets (Dark Mode & Light Mode)
Tiện ích hỗ trợ tùy biến Theme cho toàn bộ hệ thống gợi ý và soạn thảo Snippet:
- **Tùy chọn:** Bạn có thể chọn giữa **Dark** (chế độ tối mặc định) hoặc **Light** (chế độ sáng) tại Tab 1 (General Settings) hoặc trực tiếp tại thanh công cụ Tab 2 (Snippet Manager).
- **Phạm vi áp dụng đồng bộ:**
  1. **Snippet Suggestion Popup:** Khi gõ từ khóa trong editor SSMS, bảng gợi ý và khung preview code sẽ tự động đổi màu tương ứng với theme đã chọn (Dark nền than/xám hiện đại, Light nền trắng tinh tế viền xanh).
  2. **Trình soạn thảo Snippet chi tiết:** Khung soạn thảo mẫu code tại Tab 2 tự động đổi nền đen/chữ sáng hoặc nền trắng/chữ tối theo lựa chọn của bạn.
  3. **Cửa sổ tạo nhanh Snippet (Ctrl + Alt + S):** Editor code của cửa sổ pop-up nhanh cũng lập tức phản ánh theme tương ứng.

---

## 5. HƯỚNG DẪN GỠ CÀI ĐẶT SẠCH 100% (CLEAN UNINSTALL)

> [!IMPORTANT]
> **Vì sao SSMS 22 khó gỡ sạch bằng tay?**  
> SSMS 22 lưu extension trong thư mục `%LOCALAPPDATA%\Microsoft\SSMS\22.0_<id>\Extensions\`. Khi cài qua VSIXInstaller, hệ thống tạo ra một thư mục ngẫu nhiên (ví dụ `nu3wqpv4.dhq`) và lưu cache vào `ComponentModelCache`. Nếu chỉ xóa thư mục tên cố định, SSMS vẫn nạp extension từ thư mục ngẫu nhiên đó qua MEF Cache.

Bạn có thể chọn 1 trong 2 cách sau để gỡ bỏ sạch sẽ 100%:

### Cách 1: Bấm nút trên Giao diện (Tools -> Options)
1. Vào **Tools** -> **Options** -> **Lumi SQL Formatter**.
2. Bấm nút màu đỏ: **🗑 Gỡ Cài Đặt Extension**.
3. Hộp thoại xác nhận sẽ hỏi:
   - Có muốn đóng SSMS để gỡ bỏ không? -> Chọn **Yes**.
   - Có muốn xóa cả thư mục snippets cá nhân không? -> Chọn **Yes** (nếu muốn xóa hết) hoặc **No** (để giữ lại snippets).
4. Hệ thống sẽ tự động quét, xóa sạch toàn bộ file DLL (kể cả trong các thư mục ngẫu nhiên của VSIXInstaller), xóa `ComponentModelCache` và cập nhật lại SSMS.

---

### Cách 2: Chạy file `Uninstall.bat` (Đã hỗ trợ Unicode đầy đủ)
1. Đóng SSMS.
2. Vào thư mục gốc của dự án: `d:\LumiSqlFormatter\`.
3. Nhấp đúp vào file:
   ```text
   d:\LumiSqlFormatter\Uninstall.bat
   ```
4. Script sẽ tự động yêu cầu quyền Administrator, hiển thị giao diện tiếng Việt UTF-8 chuẩn xác, đóng SSMS và dọn sạch mọi vết tích extension trên toàn hệ thống trong vòng 3 giây.

---

## 6. CẤU TRÚC KỸ THUẬT CỦA DỰ ÁN

```text
d:\LumiSqlFormatter\
│
├── LumiSqlFormatter.sln                    # Solution Visual Studio 2022
├── Uninstall.bat                           # Script khởi chạy gỡ cài đặt (UTF-8)
├── Uninstall.ps1                           # Script PowerShell dọn dẹp chuyên sâu & xóa MEF Cache
├── HUONG_DAN_SU_DUNG.md                    # Tài liệu hướng dẫn sử dụng này
│
└── LumiSqlFormatter/
    ├── source.extension.vsixmanifest       # Cấu hình target SSMS 19, 20, 22 (Isolated Shell)
    ├── PostProcessVsix.ps1                 # Script biến đổi VSIX thành chuẩn thuần VsixV2 cho SSMS
    │
    ├── Models/
    │   ├── FormatterSettings.cs            # Model lưu cấu hình casing và đường dẫn snippet
    │   └── SnippetItem.cs                  # Model snippet chuẩn: id, prefix, description, body
    │
    ├── Services/
    │   ├── SettingsService.cs              # Quản lý đọc/ghi Settings.json và thư mục snippets
    │   ├── SnippetService.cs               # Logic match shortcut, suggestion, expand $CURSOR$
    │   └── SqlTokenParsingService.cs       # Bộ phân tích từ khóa T-SQL hiệu năng cao
    │
    ├── Editor/
    │   ├── KeyCommandFilter.cs             # Bộ lọc phím thời gian thực (Tab, Enter, Space,...)
    │   ├── SqlTextViewCreationListener.cs  # Đăng ký CommandFilter vào TextEditor của SSMS
    │   └── QuickSnippetCommandPackage.cs   # Đăng ký phím tắt Ctrl+Alt+S vào Visual Studio/SSMS
    │
    └── UI/
        ├── ViewModels/
        │   ├── SettingsViewModel.cs        # ViewModel cho Options Page và nút Gỡ cài đặt
        │   └── QuickSnippetViewModel.cs    # ViewModel cho cửa sổ tạo nhanh snippet
        └── Views/
            ├── SettingsView.xaml           # Giao diện Tools -> Options
            ├── QuickSnippetWindow.xaml     # Cửa sổ Ctrl+Alt+S
            └── SnippetSuggestionPopup.xaml # Popup gợi ý snippet khi gõ code
```

---
*Chúc bạn có trải nghiệm viết mã T-SQL mượt mà và năng suất cùng Lumi SQL Formatter!*
