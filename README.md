# Lumi SQL Formatter & Smart Snippets (PHONGNT)

Extension chuyên nghiệp cho **SQL Server Management Studio (SSMS 18, 19, 20, 22+)** (Visual Studio Shell x64), được thiết kế lấy cảm hứng từ các tính năng cốt lõi của **Redgate SQL Prompt**:

1. **Auto-Capitalization thời gian thực:** Tự động chuyển đổi từ khóa khi gõ kết thúc từ (`Space`, `Enter`, `Tab`, `,`, `;`, `)`) theo các chuẩn `UPPERCASE`, `lowercase`, hoặc `PascalCase`.
2. **Bộ từ điển 510+ Từ Khóa & Hàm T-SQL:** Bao gồm đầy đủ câu lệnh DQL/DML/DDL (kèm `COLUMN`), Window Functions, Pivot, Transaction, Hints, SET Options, 37 Data Types và 180+ Built-in Functions.
3. **Strict Multi-line Exclusions:** Bộ phân tích từ vựng (Lexical State Machine) trên Buffer Snapshot loại trừ an toàn: chuỗi ký tự nhiều dòng (`'...'`, `N'...'`), escape nháy kép (`''`), comment lồng nhau (`/* ... */`), và định danh trong ngoặc vuông (`[...]`, `"..."`).
4. **Bảo vệ biến T-SQL & Bảng tạm:** Các biến cục bộ `@Year`, `@Date`, `@Status`... và bảng tạm `#Year` không bao giờ bị viết hoa nhầm thành từ khóa SQL.
5. **Smart Snippet Manager:**
   - Kích hoạt nhanh template bằng phím `Tab` hoặc `Enter` khi gõ prefix (ví dụ: `ssf`, `adrop`, `ct`, `ii`...). Hỗ trợ vị trí đặt con trỏ cuối cùng `$CURSOR$`.
   - **Tùy chỉnh thư mục Snippet (Custom Directory):** Cho phép trỏ tới bất kỳ thư mục nào (OneDrive, Git repo, Shared drive...). Lưu dưới dạng từng file `{prefix}.json` độc lập (Redgate style).
   - **Khung nhập nhanh Snippet từ Editor (`Ctrl+Alt+S`):** Bôi đen đoạn code bất kỳ trong Query Editor, nhấn `Ctrl+Alt+S` để mở modal tạo nhanh template ngay lập tức.
   - **Suggestion Popup khi gõ:** Popup IntelliSense nổi lên ngay cạnh con trỏ soạn thảo hiển thị danh sách gợi ý và xem trước code template.
6. **Giao diện quản trị hiện đại (WPF UI):** Tích hợp vào **Tools -> Options -> Lumi SQL Formatter**, hỗ trợ lưới 3 dòng thu gọn tối ưu không gian, giao diện Dark Mode / Light Mode sắc nét và nút Làm mới (Refresh).

---

## 1. Cấu Trúc Dự Án

```text
d:\LumiSqlFormatter\
├── LumiSqlFormatter.sln                        # Visual Studio Solution
├── Install-To-SSMS22.bat                       # 1-Click cài đặt/cập nhật tự động vào SSMS
├── Uninstall.bat / Uninstall.ps1               # Gỡ cài đặt sạch 100%
├── HUONG_DAN_SU_DUNG.md                        # Hướng dẫn chi tiết toàn diện
├── README.md                                   # Tài liệu tóm tắt
└── LumiSqlFormatter\
    ├── LumiSqlFormatter.csproj                 # Cấu hình VSSDK, SSMS References
    ├── source.extension.vsixmanifest           # VSIX Manifest (v1.1.2 - PHONGNT)
    ├── ExtensionPackage.cs                     # AsyncPackage đăng ký Top-Menu, ToolWindow, OptionsPage
    ├── ExtensionPackage.vsct                   # Định nghĩa Top-Level Menu "Lumi SQL"
    ├── PostProcessVsix.ps1                     # Đóng gói chuẩn SSMS VsixV2
    ├── Models/                                 # Models CasingStyle, FormatterSettings, SnippetItem
    ├── Services/                               # SettingsService, SqlTokenParsingService, SnippetService
    ├── Editor/                                 # TextViewCreationListener, KeyCommandFilter
    ├── UI/                                     # SettingsToolWindow, OptionsPage, Views, ViewModels
    └── Properties/                             # AssemblyInfo.cs (v1.1.2.0)
```

---

## 2. Hướng Dẫn Cài Đặt Nhanh

1. Đóng hoàn toàn SSMS.
2. Tìm đến file `.vsix` tại:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. Nhấp đúp chuột vào file để cài đặt trực tiếp qua `VSIX Installer`.
4. Mở lại SSMS để trải nghiệm.

> [!NOTE]
> Để xem chi tiết hướng dẫn sử dụng tất cả tính năng, danh mục 510+ từ khóa, và nhật ký các phiên bản, vui lòng xem tại: [HUONG_DAN_SU_DUNG.md](file:///d:/LumiSqlFormatter/HUONG_DAN_SU_DUNG.md).
