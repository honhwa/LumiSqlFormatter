# Lumi SQL Formatter & Smart Snippets (SSMS 22 Extension)

Extension mã nguồn mở cho **SQL Server Management Studio 22 (SSMS 22)** (Visual Studio Shell x64), được thiết kế lấy cảm hứng từ các tính năng cốt lõi của **Redgate SQL Prompt**:
1. **Auto-Capitalization thời gian thực:** Tự động chuyển đổi từ khóa khi gõ kết thúc từ (`Space`, `Enter`, `Tab`, `,`, `;`) theo các chuẩn `UPPERCASE`, `lowercase`, hoặc `PascalCase`.
2. **Strict Exclusions:** Áp dụng bộ phân tích cú pháp `Microsoft.SqlServer.TransactSql.ScriptDom` Lexer để loại trừ an toàn: không format bên trong chuỗi ký tự (`'...'`, `N'...'`), comment (`--`, `/* */`), và định danh được bọc (`[...]`, `"..."`).
3. **Smart Snippet Manager:** 
   - Kích hoạt nhanh template bằng phím `Tab` khi gõ shortcut (ví dụ: `ssf`, `ct`, `ii`, `ud`, `cproc`...). Hỗ trợ vị trí đặt con trỏ cuối cùng `$CURSOR$` / `$CARET$` và các placeholder biến (`$TABLE_NAME$`, `$COLUMN$`...).
   - **Tùy chỉnh thư mục Snippet (Custom Directory):** Cho phép trỏ tới bất kỳ thư mục nào (OneDrive, Git repo, Shared drive...). Hỗ trợ đọc/ghi cả file tổng hợp lẫn file snippet `.json` riêng lẻ (Redgate style).
   - **Khung nhập nhanh Snippet từ Editor (`Ctrl+Alt+S`):** Bôi đen đoạn code bất kỳ trong Query Editor, nhấn `Ctrl+Alt+S` để mở modal tạo nhanh template ngay lập tức.
   - **Suggestion Popup khi gõ:** Khi gõ các ký tự bắt đầu của shortcut, một popup IntelliSense nổi lên ngay cạnh con trỏ soạn thảo hiển thị danh sách gợi ý, xem trước code template, dùng phím `Up`/`Down` điều hướng và `Tab`/`Enter` để chọn.
4. **Giao diện tùy chỉnh (WPF UI):** Tích hợp Top Menu `Lumi SQL` trên SSMS và `Tools -> Options`, hỗ trợ Live Real-time Preview cập nhật ngay lập tức khi thay đổi tùy chọn casing và trình quản lý CRUD Snippet đầy đủ (kèm tính năng Import/Export JSON).

---

## 1. Cấu Trúc Dự Án

```
d:\LumiSqlFormatter\
├── LumiSqlFormatter.sln                        # Visual Studio Solution
├── Install-To-SSMS22.bat                       # 1-Click cài đặt/cập nhật tự động vào SSMS 22
├── Deploy-To-SSMS22.ps1                        # PowerShell script triển khai
├── README.md                                   # Hướng dẫn chi tiết
└── LumiSqlFormatter\
    ├── LumiSqlFormatter.csproj                 # Cấu hình VSSDK, SSMS 22 StartProgram, References
    ├── source.extension.vsixmanifest           # VSIX Manifest hỗ trợ SSMS 22 (x64)
    ├── ExtensionPackage.cs                     # AsyncPackage đăng ký Top-Menu, ToolWindow, OptionsPage
    ├── ExtensionPackage.vsct                   # Định nghĩa Top-Level Menu "Lumi SQL"
    ├── Guids.cs & PkgCmdIDList.cs              # GUIDs và Command IDs
    ├── Models/
    │   ├── CasingStyle.cs                      # Enum: Uppercase, Lowercase, PascalCase
    │   ├── FormatterSettings.cs                # Model cài đặt Casing & Snippets
    │   └── SnippetItem.cs                      # Model Snippet (Shortcut, Description, Template)
    ├── Services/
    │   ├── SettingsService.cs                  # Quản lý đọc/ghi JSON tại folder tùy chỉnh
    │   ├── SqlTokenParsingService.cs           # ScriptDom Lexer & O(1) hashmap phân loại token
    │   └── SnippetService.cs                   # Mở rộng template, tính toán vị trí $CURSOR$ & gợi ý prefix
    ├── Editor/
    │   ├── TextViewCreationListener.cs         # MEF Component IVsTextViewCreationListener
    │   └── KeyCommandFilter.cs                 # IOleCommandTarget bắt phím Tab, Space, Enter, Up, Down, v.v.
    ├── UI/
    │   ├── SettingsToolWindow.cs               # ToolWindow nhúng SettingsView
    │   ├── OptionsPage.cs                      # DialogPage trong Tools -> Options -> Lumi SQL Formatter
    │   ├── Views/
    │   │   ├── SettingsView.xaml               # WPF Modern UI (Formatting Tab & Snippets Tab)
    │   │   ├── QuickSnippetWindow.xaml         # Modal tạo nhanh snippet từ editor
    │   │   └── SnippetSuggestionPopup.xaml     # Popup gợi ý snippet cạnh con trỏ soạn thảo
    │   └── ViewModels/
    │       ├── ViewModelBase.cs                # MVVM INotifyPropertyChanged
    │       ├── RelayCommand.cs                 # ICommand implementation
    │       ├── QuickSnippetViewModel.cs        # ViewModel cho Quick Snippet Creator
    │       └── SettingsViewModel.cs            # Binding logic, live preview, CRUD snippets
    └── Properties/
        └── AssemblyInfo.cs
```

---

## 2. Hướng Dẫn Sử Dụng Các Tính Năng Mới

### 1. Tùy Chỉnh Thư Mục Snippet (Custom Snippets Folder)
1. Mở menu **`Lumi SQL -> Settings & Snippet Manager...`** (hoặc `Tools -> Options -> Lumi SQL Formatter`).
2. Chuyển sang tab **Snippet Manager**.
3. Tại thanh **📁 Thư mục Snippets**:
   - Bấm **Browse...** để chọn thư mục lưu trữ bất kỳ (ví dụ: `D:\Work\MySnippets` hoặc thư mục OneDrive đồng bộ).
   - Bấm **Mở thư mục** để xem trực tiếp các file `.json` trong Windows Explorer.
   - Bấm **🔄 Nạp lại** để tải lại các snippet nếu bạn vừa pull từ Git hoặc chỉnh sửa ngoài file.

### 2. Tạo Nhanh Snippet Từ Editor (`Ctrl+Alt+S`)
1. Trong cửa sổ Query Editor của SSMS 22, bôi đen đoạn SQL bạn vừa viết xong.
2. Nhấn tổ hợp phím **`Ctrl + Alt + S`**.
3. Cửa sổ **⚡ Quick Create Snippet** sẽ mở ra với đoạn code đã được điền sẵn:
   - Gõ mã shortcut gợi nhớ (ví dụ: `custq`).
   - Nhập mô tả.
   - Bấm nút **📍 Chèn $CURSOR$** vào vị trí bạn muốn con trỏ chuột dừng lại khi bung template sau này.
   - Nhấn **`Enter`** để Lưu. Snippet có thể dùng được ngay lập tức!

### 3. Suggestion Popup Khi Gõ Snippet
1. Mở cửa sổ soạn thảo SQL Query.
2. Bắt đầu gõ các chữ cái đầu của snippet (ví dụ: gõ `s` hoặc `c`).
3. Một danh sách gợi ý thông minh sẽ hiển thị nổi ngay dưới con trỏ soạn thảo:
   - Cột trái: Danh sách các snippet phù hợp kèm badge shortcut nổi bật.
   - Cột phải: Xem trước code template hoàn chỉnh sẽ được bung ra.
   - Phím **`Up` / `Down`**: Di chuyển chọn snippet khác trong danh sách.
   - Phím **`Tab` hoặc `Enter`**: Chọn snippet đang highlight để bung toàn bộ code.
   - Phím **`Esc`**: Đóng popup gợi ý.

---

## 3. Cài Đặt & Cập Nhật Vào SSMS 22 (1-Click)

Chỉ cần chạy file:
```cmd
d:\LumiSqlFormatter\Install-To-SSMS22.bat
```
File này sẽ tự động:
1. Đóng tiến trình SSMS nếu đang chạy.
2. Giải nén toàn bộ VSIX bản mới nhất vào thư mục Extensions của SSMS 22.
3. Làm sạch ComponentModelCache.
4. Chạy `SSMS.exe /updateconfiguration` để hoàn tất cập nhật.
