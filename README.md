# Lumi SQL Formatter & Smart Snippets (PHONGNT)

Extension chuyên nghiệp cho **SQL Server Management Studio (SSMS 18, 19, 20, 22+)** (Visual Studio Shell x64), được thiết kế lấy cảm hứng từ các tính năng cốt lõi của **Redgate SQL Prompt**:

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

## 1. Cấu Trúc Dự Án

```text
d:\LumiSqlFormatter\
├── LumiSqlFormatter.sln                        # Visual Studio Solution
├── HUONG_DAN_SU_DUNG.md                        # Tài liệu hướng dẫn sử dụng chi tiết
├── README.md                                   # Giới thiệu tổng quan dự án
└── LumiSqlFormatter\
    ├── LumiSqlFormatter.csproj                 # Cấu hình VSSDK, SSMS References
    ├── source.extension.vsixmanifest           # VSIX Manifest (v1.1.3 - PHONGNT)
    ├── ExtensionPackage.cs                     # AsyncPackage đăng ký Top-Menu, ToolWindow, OptionsPage
    ├── ExtensionPackage.vsct                   # Định nghĩa Top-Level Menu "Lumi SQL"
    ├── PostProcessVsix.ps1                     # Đóng gói chuẩn SSMS VsixV2
    ├── Models/                                 # Models CasingStyle, FormatterSettings, SnippetItem
    ├── Services/                               # SettingsService, SqlTokenParsingService, SnippetService
    ├── Editor/                                 # TextViewCreationListener, KeyCommandFilter
    ├── UI/                                     # SettingsToolWindow, OptionsPage, Views, ViewModels
    └── Properties/                             # AssemblyInfo.cs (v1.1.3.0)
```

---

## 2. Hướng Dẫn Cài Đặt Qua File .VSIX

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
