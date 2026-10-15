# Báo Cáo Cập Nhật Phiên Bản 1.1.2: Bổ Sung Keyword COLUMN & Xử Lý Biến (@Year)

## 1. Nội Dung Cập Nhật Trong Phiên Bản 1.1.2

### 1.1. Bổ sung từ khóa `COLUMN`
- Đã thêm `("COLUMN", "Column")` vào danh mục Reserved Keyword trong [`SqlTokenParsingService.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/Services/SqlTokenParsingService.cs).
- Khi gõ các câu lệnh DDL như:
  ```sql
  ALTER TABLE MyTable ADD COLUMN MyCol INT;
  ALTER TABLE MyTable DROP COLUMN MyCol;
  ```
  Từ `column` sẽ tự động được in hoa thành `COLUMN`.

### 1.2. Xử lý triệt để ngoại lệ biến T-SQL (`@Year`)
- **Hiện tượng trước đây:** Khi người dùng khai báo hoặc sử dụng biến có tên trùng với từ khóa/hàm T-SQL (như `@Year`, `@Date`, `@Month`, `@Status`, `@Type`, v.v.), cơ chế tách từ lấy được từ `Year`, kiểm tra thấy `YEAR` có trong danh mục hàm thời gian và tự động in hoa thành `@YEAR`.
- **Giải pháp đã thực hiện trong [`KeyCommandFilter.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/Editor/KeyCommandFilter.cs):**
  - Tại cả 2 hàm xử lý `FormatWordPrecedingCaret()` (khi gõ phím phân cách) và `FormatWordOnPreviousLineAfterReturn()` (khi nhấn Enter):
    ```csharp
    // Exclude T-SQL user variables: e.g. @Year, @Date, @Status (single @ prefix) and temporary tables (#Year)
    if (startIdx > 0 && ((lineText[startIdx - 1] == '@' && (startIdx < 2 || lineText[startIdx - 2] != '@')) || lineText[startIdx - 1] == '#'))
        return;
    ```
  - **Kết quả:**
    - Khi gõ `@Year`, `@year`, `@date`, `@month`, `@type` $\rightarrow$ giữ nguyên đúng dạng biến do người dùng gõ (không bị ép in hoa).
    - Khi dùng hàm `YEAR(...)` hoặc câu lệnh `WHERE YEAR(col) = 2026` $\rightarrow$ vẫn tự động in hoa `YEAR` bình thường vì không có ký tự `@` đứng liền trước.
    - Hỗ trợ thêm bảng tạm `#Year` để không bị in hoa nhầm tên bảng tạm.
    - Hỗ trợ thêm ký tự đóng ngoặc `)` vào danh sách kích hoạt AutoUpperCase (ví dụ: `CAST(col AS int)` $\rightarrow$ gõ `)` tự động đổi thành `INT`).

---

## 2. Thông Tin Phiên Bản

- **Phiên bản giữ nguyên:** `1.1.2`
- **File VSIX đã được build Release:**
  `d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix`
- **Trạng thái Build:** `0 Warning(s), 0 Error(s)`.
