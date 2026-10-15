# Báo Cáo Cập Nhật Lumi SQL Formatter

## 1. Tổng Quan Thay Đổi
Bản cập nhật giải quyết 2 vấn đề trọng tâm theo yêu cầu:
1. **Sửa dứt điểm lỗi không nhập liệu / gõ phím được** tại các ô **Prefix**, **Mô tả** và **Nội dung Body (Template)** trong Trình Soạn Thảo Snippet Chi Tiết (`Tools -> Options -> Lumi SQL Formatter -> General -> Tab 2: Snippet Manager`).
2. **Thu gọn bảng danh sách Snippet Grid xuống đúng 3 dòng** để tối ưu không gian chiều dọc, nhường toàn bộ diện tích cho khung soạn thảo code.

---

## 2. Chi Tiết Các Giải Pháp Kỹ Thuật

### A. Sửa Lỗi Không Nhập Liệu Trong Dialog Modal của SSMS
- **Nguyên nhân gốc rễ:** 
  - Hộp thoại `Tools -> Options` của Visual Studio / SSMS là Win32 Modal Dialog được điều khiển bởi hàm vòng lặp tin nhắn `IsDialogMessage`.
  - Khi control WPF nằm bên trong DialogPage, nếu trả về cờ `DLGC_WANTALLKEYS` một cách bừa bãi, hàm `IsDialogMessage` sẽ chặn và không gọi `TranslateMessage(&msg)`.
  - Khi thiếu `TranslateMessage`, hệ điều hành Windows **không bao giờ phát sinh thông điệp `WM_CHAR`**. Do WPF TextBoxes phụ thuộc hoàn toàn vào `WM_CHAR` để kích hoạt sự kiện nhập text (`TextInputEvent`), người dùng hoàn toàn không thể gõ được ký tự nào vào ô text.
- **Giải pháp:**
  - **Sử dụng `UIElementDialogPage`**: Chuyển [`OptionsPage.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/OptionsPage.cs) sang kế thừa trực tiếp từ `UIElementDialogPage`, trả về `_settingsView` qua `protected override UIElement Child`. Cơ chế này tích hợp bộ lọc phím `IKeyboardInputSink` chuẩn mực của Visual Studio Shell.
  - **Viết lại bộ hook `WM_GETDLGCODE` chuyên sâu**: Trong [`SettingsView.xaml.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/Views/SettingsView.xaml.cs):
    ```csharp
    // Đối với các TextBox: chỉ yêu cầu CHARS, SETSEL và ARROWS, KHÔNG bật DLGC_WANTALLKEYS
    uint flags = DLGC_WANTCHARS | DLGC_HASSETSEL | DLGC_WANTARROWS;

    // Chỉ bật WANTTAB khi ô đó cho phép tab (TemplateTextBox với AcceptsTab = true)
    if (tb.AcceptsTab)
    {
        flags |= DLGC_WANTTAB;
    }

    // Đối với ô nhiều dòng: giữ phím Enter (VK_RETURN) để xuống dòng mà không submit form
    if (tb.AcceptsReturn && lParam != IntPtr.Zero)
    {
        int wParamOffset = IntPtr.Size == 8 ? 16 : IntPtr.Size + 4;
        int vk = Marshal.ReadInt32(lParam, wParamOffset);
        if (vk == 0x0D)
        {
            flags |= DLGC_WANTALLKEYS;
        }
    }
    ```
  - **Đồng bộ Focus Win32 tự động (`EnsureWin32Focus`)**:
    - Khi người dùng bấm **➕ Thêm mới**, hệ thống sẽ tự động cuộn đến item mới, gọi `SetFocus(HwndSource.Handle)`, kích hoạt focus vào ô `PrefixTextBox` và **tự động bôi đen toàn bộ chữ `new_prefix`** để người dùng có thể gõ ngay lập tức mà không cần click thêm chuột.
    - Bổ sung sự kiện `PreviewMouseLeftButtonDown` cho các ô TextBoxes để bảo đảm mỗi khi click chuột vào bất kỳ ô nào, HWND con của WPF lập tức nhận Win32 focus.

---

### B. Thu Gọn Snippet Grid Xuống Đúng 3 Dòng
- **Tối ưu layout trong [`SettingsView.xaml`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/Views/SettingsView.xaml):**
  - Cấu hình kích thước chính xác cho DataGrid:
    - `Height="108"`: Tương ứng 1 dòng Header (`ColumnHeaderHeight="28"`) + 3 dòng dữ liệu (`RowHeight="26" * 3 = 78`) + 2px borders.
    - `VerticalScrollBarVisibility="Auto"`: Khi có nhiều hơn 3 snippets, thanh cuộn tự động xuất hiện bên trong mà không làm giãn nở giao diện.
    - `CanUserSortColumns="False"`: Khóa tính năng bấm tiêu đề cột để sắp xếp, loại bỏ tình trạng danh sách tự đảo lộn vị trí khi đang sửa `Prefix`.
  - Khung **Trình Soạn Thảo Snippet Chi Tiết** (`Row 3`) được đặt `RowDefinition Height="*"` để co giãn chiếm toàn bộ phần diện tích còn lại của màn hình full screen, mang lại trải nghiệm viết code SQL template thoải mái nhất.

---

## 3. Các Tệp Mã Nguồn Đã Chỉnh Sửa

| Tệp tin | Vị trí thay đổi chính | Mô tả |
| :--- | :--- | :--- |
| [`OptionsPage.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/OptionsPage.cs) | Dòng 15, 72-107 | Kế thừa `UIElementDialogPage`, trả về `Child`, điều phối focus Win32 trong `OnActivate`. |
| [`SettingsView.xaml.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/Views/SettingsView.xaml.cs) | Dòng 72-195 | Hook `WM_GETDLGCODE`, `EnsureWin32Focus()`, tự động focus & select all ô `Prefix` khi bấm Thêm mới. |
| [`SettingsView.xaml`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/Views/SettingsView.xaml) | Dòng 186-250 | Cố định `Height="108"`, `RowHeight="26"`, `ColumnHeaderHeight="28"`; mở rộng khung soạn thảo template. |
| [`SettingsViewModel.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/UI/ViewModels/SettingsViewModel.cs) | Dòng 440-449 | Bắn event `SnippetAdded` thông báo cho View kích hoạt focus ngay khi thêm mới. |

---

## 4. Kiểm Thử Và Đóng Gói
- **Kết quả Build MSBuild Release:** `0 Warning(s), 0 Error(s)`.
- **File cài đặt VSIX mới nhất:**
  - Đường dẫn: `d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix`
  - Đã được post-process theo chuẩn VsixV2 tương thích hoàn toàn với SSMS 22 (Isolated Shell).
