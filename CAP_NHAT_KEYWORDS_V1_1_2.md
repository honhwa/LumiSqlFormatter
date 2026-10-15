# Báo Cáo Nâng Cấp Phiên Bản 1.1.2 & Bổ Sung Toàn Diện Bộ Keyword In Hoa (AutoUpperCase)

## 1. Nâng Phiên Bản Lên 1.1.2
- **Phiên bản mới:** `1.1.2`
- **Các tệp đã cập nhật version:**
  - [`source.extension.vsixmanifest`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/source.extension.vsixmanifest): `Version="1.1.2"`
  - [`Properties\AssemblyInfo.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/Properties/AssemblyInfo.cs): `AssemblyVersion("1.1.2.0")`, `AssemblyFileVersion("1.1.2.0")`
  - [`ExtensionPackage.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/ExtensionPackage.cs): `InstalledProductRegistration("1.1.2")`
- **File VSIX đóng gói sẵn sàng:**
  `d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix`
  *(Đã post-process đóng gói chuẩn SSMS VsixV2, sẵn sàng cài đè trực tiếp)*.

---

## 2. Bổ Sung Toàn Diện Danh Sách Từ Khóa T-SQL (510 Từ Khóa & Hàm)

Trong [`SqlTokenParsingService.cs`](file:///d:/LumiSqlFormatter/LumiSqlFormatter/Services/SqlTokenParsingService.cs), từ điển từ khóa đã được mở rộng từ ~190 lên **510 từ khóa độc nhất**, bao gồm đầy đủ tất cả các nhóm cú pháp T-SQL:

### 2.1. Cú pháp & Mệnh đề SQL (Reserved Keywords & Clauses - 280+ từ):
- **DQL / DML / DDL cơ bản:** `SELECT`, `FROM`, `WHERE`, `GROUP`, `BY`, `HAVING`, `ORDER`, `ASC`, `DESC`, `INSERT`, `INTO`, `VALUES`, `VALUE`, `UPDATE`, `SET`, `DELETE`, `MERGE`, `MATCHED`, `TARGET`, `SOURCE`, `USING`, `OUTPUT`, `DELETED`, `INSERTED`, `ACTION`, `TOP`, `DISTINCT`, `PERCENT`, `TIES`.
- **Joins & Set Operators:** `JOIN`, `INNER`, `LEFT`, `RIGHT`, `FULL`, `CROSS`, `OUTER`, `APPLY`, `ON`, `UNION`, `ALL`, `EXCEPT`, `INTERSECT`.
- **Logic & Điều kiện:** `CASE`, `WHEN`, `THEN`, `ELSE`, `END`, `AND`, `OR`, `NOT`, `IN`, `IS`, `NULL`, `LIKE`, `BETWEEN`, `EXISTS`, `SOME`, `ANY`, `COLLATE`, `ESCAPE`.
- **Phân trang & Windowing:** `OVER`, `PARTITION`, `ROWS`, `RANGE`, `UNBOUNDED`, `PRECEDING`, `FOLLOWING`, `CURRENT`, `ROW`, `OFFSET`, `FETCH`, `NEXT`, `ONLY`, `FIRST`, `LAST`.
- **Pivot & Lấy mẫu:** `PIVOT`, `UNPIVOT`, `TABLESAMPLE`.
- **Giao dịch & Luồng điều khiển:** `BEGIN`, `TRAN`, `TRANSACTION`, `COMMIT`, `ROLLBACK`, `SAVE`, `EXEC`, `EXECUTE`, `DECLARE`, `AS`, `WITH`, `IF`, `WHILE`, `BREAK`, `CONTINUE`, `RETURN`, `RETURNS`, `GOTO`, `WAITFOR`, `DELAY`.
- **Xử lý lỗi:** `TRY`, `CATCH`, `THROW`, `RAISERROR`, `PRINT`.
- **Đối tượng CSDL & Quản trị:** `CREATE`, `ALTER`, `DROP`, `TRUNCATE`, `TABLE`, `VIEW`, `PROCEDURE`, `PROC`, `FUNCTION`, `TRIGGER`, `INDEX`, `DATABASE`, `SCHEMA`, `SEQUENCE`, `SYNONYM`, `TYPE`, `RULE`, `DEFAULT`, `USER`, `ROLE`, `LOGIN`, `CERTIFICATE`, `SYMMETRIC`, `ASYMMETRIC`, `KEY`, `CREDENTIAL`, `BACKUP`, `RESTORE`, `DBCC`, `BULK`, `SHUTDOWN`, `KILL`, `ENABLE`, `DISABLE`, `EXTERNAL`, `DISK`.
- **Ràng buộc & Chỉ mục:** `PRIMARY`, `FOREIGN`, `REFERENCES`, `CONSTRAINT`, `CHECK`, `UNIQUE`, `CLUSTERED`, `NONCLUSTERED`, `COLUMNSTORE`, `CASCADE`, `RESTRICT`, `NOCHECK`, `CHECKPOINT`, `INCLUDE`, `FILLFACTOR`, `PAD_INDEX`, `STATISTICS_NORECOMPUTE`, `IGNORE_DUP_KEY`, `ALLOW_ROW_LOCKS`, `ALLOW_PAGE_LOCKS`, `DATA_COMPRESSION`, `PAGE`, `NONE`, `ONLINE`, `OFFLINE`, `SORT_IN_TEMPDB`, `MAXDOP`, `PERSISTED`, `SPARSE`, `FILEGROUP`, `IDENTITY`, `IDENTITY_INSERT`, `ROWGUIDCOL`.
- **Query Hints & Lock Hints:** `OPTION`, `RECOMPILE`, `MAXRECURSION`, `OPTIMIZE`, `FOR`, `UNKNOWN`, `FAST`, `FORCE`, `EXPAND`, `VIEWS`, `LOOP`, `HASH`, `NOLOCK`, `READUNCOMMITTED`, `READCOMMITTED`, `REPEATABLEREAD`, `SERIALIZABLE`, `READCOMMITTEDLOCK`, `READPAST`, `XLOCK`, `UPDLOCK`, `ROWLOCK`, `PAGLOCK`, `TABLOCK`, `TABLOCKX`, `HOLDLOCK`, `NOWAIT`, `KEEPIDENTITY`, `KEEPDEFAULTS`.
- **Phân quyền & Bảo mật:** `GRANT`, `DENY`, `REVOKE`, `TO`, `AUTHORIZATION`, `REVERT`, `SECURITYAUDIT`, `ENCRYPTION`, `SCHEMABINDING`, `VIEW_METADATA`, `CALLER`, `SELF`, `OWNER`.
- **Thiết lập phiên làm việc (SET Options):** `NOCOUNT`, `XACT_ABORT`, `ANSI_NULLS`, `QUOTED_IDENTIFIER`, `ARITHABORT`, `NUMERIC_ROUNDABORT`, `CONCAT_NULL_YIELDS_NULL`, `ANSI_PADDING`, `ANSI_WARNINGS`, `CURSOR_CLOSE_ON_COMMIT`, `IMPLICIT_TRANSACTIONS`, `ISOLATION`, `LEVEL`, `SNAPSHOT`, `FORCEPLAN`, `SHOWPLAN_ALL`, `SHOWPLAN_TEXT`, `SHOWPLAN_XML`, `STATISTICS`, `IO`, `PROFILE`, `OFF`, `ON`, `GO`, `USE`, `RECONFIGURE`.
- **Cursor:** `CURSOR`, `OPEN`, `CLOSE`, `DEALLOCATE`, `GLOBAL`, `LOCAL`, `FORWARD_ONLY`, `SCROLL`, `STATIC`, `KEYSET`, `DYNAMIC`, `FAST_FORWARD`, `READ_ONLY`, `SCROLL_LOCKS`, `OPTIMISTIC`, `TYPE_WARNING`.
- **XML / JSON / Full-Text:** `PATH`, `AUTO`, `EXPLICIT`, `RAW`, `ELEMENTS`, `ROOT`, `OPENROWSET`, `OPENDATASOURCE`, `OPENQUERY`, `OPENXML`, `OPENJSON`, `CONTAINS`, `FREETEXT`, `CONTAINSTABLE`, `FREETEXTTABLE`, `READTEXT`, `UPDATETEXT`, `WRITETEXT`, `TEXTSIZE`.
- **Tham số Procedure:** `OUT`, `READONLY`, `VARYING`.

### 2.2. Kiểu dữ liệu T-SQL (Data Types - 37 kiểu):
- `INT`, `INTEGER`, `BIGINT`, `SMALLINT`, `TINYINT`, `BIT`
- `DECIMAL`, `NUMERIC`, `MONEY`, `SMALLMONEY`, `FLOAT`, `REAL`
- `DATE`, `DATETIME`, `DATETIME2`, `DATETIMEOFFSET`, `SMALLDATETIME`, `TIME`
- `CHAR`, `CHARACTER`, `VARCHAR`, `NCHAR`, `NVARCHAR`, `TEXT`, `NTEXT`
- `BINARY`, `VARBINARY`, `IMAGE`
- `UNIQUEIDENTIFIER`, `XML`, `SQL_VARIANT`, `GEOMETRY`, `GEOGRAPHY`, `HIERARCHYID`, `ROWVERSION`, `TIMESTAMP`, `SYSNAME`, `JSON`, `VECTOR`

### 2.3. Các hàm tích hợp & Hệ thống (Built-in & System Functions - 180+ hàm):
- **Tập hợp (Aggregate):** `COUNT`, `COUNT_BIG`, `SUM`, `AVG`, `MIN`, `MAX`, `STRING_AGG`, `APPROX_COUNT_DISTINCT`, `CHECKSUM_AGG`, `STDEV`, `STDEVP`, `VAR`, `VARP`, `GROUPING`, `GROUPING_ID`.
- **Xếp hạng & Window:** `ROW_NUMBER`, `RANK`, `DENSE_RANK`, `NTILE`, `LEAD`, `LAG`, `FIRST_VALUE`, `LAST_VALUE`, `PERCENT_RANK`, `CUME_DIST`, `PERCENTILE_CONT`, `PERCENTILE_DISC`.
- **Chuỗi:** `ASCII`, `CHAR`, `CHARINDEX`, `CONCAT`, `CONCAT_WS`, `DIFFERENCE`, `FORMAT`, `LEFT`, `LEN`, `LOWER`, `LTRIM`, `NCHAR`, `PATINDEX`, `QUOTENAME`, `REPLACE`, `REPLICATE`, `REVERSE`, `RIGHT`, `RTRIM`, `SOUNDEX`, `SPACE`, `STR`, `STRING_ESCAPE`, `STRING_SPLIT`, `STUFF`, `SUBSTRING`, `TRANSLATE`, `TRIM`, `UNICODE`, `UPPER`, `DATALENGTH`.
- **Ngày giờ:** `GETDATE`, `GETUTCDATE`, `SYSDATETIME`, `SYSUTCDATETIME`, `SYSDATETIMEOFFSET`, `DATEADD`, `DATEDIFF`, `DATEDIFF_BIG`, `DATEPART`, `DATENAME`, `DATEFROMPARTS`, `DATETIME2FROMPARTS`, `DATETIMEFROMPARTS`, `DATETIMEOFFSETFROMPARTS`, `SMALLDATETIMEFROMPARTS`, `TIMEFROMPARTS`, `EOMONTH`, `ISDATE`, `SWITCHOFFSET`, `TODATETIMEOFFSET`, `YEAR`, `MONTH`, `DAY`.
- **Chuyển đổi kiểu:** `CAST`, `CONVERT`, `PARSE`, `TRY_CAST`, `TRY_CONVERT`, `TRY_PARSE`.
- **Logic & Điều kiện:** `ISNULL`, `COALESCE`, `NULLIF`, `IIF`, `CHOOSE`, `GREATEST`, `LEAST`.
- **Toán học:** `ABS`, `ACOS`, `ASIN`, `ATAN`, `ATN2`, `CEILING`, `COS`, `COT`, `DEGREES`, `EXP`, `FLOOR`, `LOG`, `LOG10`, `PI`, `POWER`, `RADIANS`, `RAND`, `ROUND`, `SIGN`, `SIN`, `SQRT`, `SQUARE`, `TAN`.
- **Metadata & Đối tượng:** `SCOPE_IDENTITY`, `IDENT_CURRENT`, `IDENT_INCR`, `IDENT_SEED`, `OBJECT_ID`, `OBJECT_NAME`, `OBJECT_SCHEMA_NAME`, `OBJECT_DEFINITION`, `OBJECTPROPERTY`, `OBJECTPROPERTYEX`, `COLUMNPROPERTY`, `COL_LENGTH`, `COL_NAME`, `INDEXPROPERTY`, `INDEXKEY_PROPERTY`, `INDEX_COL`, `DATABASEPROPERTYEX`, `DB_ID`, `DB_NAME`, `SCHEMA_ID`, `SCHEMA_NAME`, `TYPE_ID`, `TYPE_NAME`, `SERVERPROPERTY`, `STATS_DATE`.
- **Người dùng & Phiên:** `USER_ID`, `USER_NAME`, `SUSER_ID`, `SUSER_NAME`, `SUSER_SID`, `SUSER_SNAME`, `ORIGINAL_LOGIN`, `ORIGINAL_DB_NAME`, `HAS_PERMS_BY_NAME`, `IS_SRVROLEMEMBER`, `IS_MEMBER`, `IS_ROLEMEMBER`, `APP_NAME`, `HOST_NAME`, `HOST_ID`, `NEWID`, `NEWSEQUENTIALID`, `XACT_STATE`, `CURRENT_TIMESTAMP`, `CURRENT_USER`, `SESSION_USER`, `SYSTEM_USER`.
- **Thông tin Lỗi:** `ERROR_MESSAGE`, `ERROR_NUMBER`, `ERROR_SEVERITY`, `ERROR_STATE`, `ERROR_LINE`, `ERROR_PROCEDURE`.
- **JSON:** `ISJSON`, `JSON_VALUE`, `JSON_QUERY`, `JSON_MODIFY`, `JSON_PATH_EXISTS`, `JSON_ARRAY`, `JSON_OBJECT`.
- **Biến vô hướng hệ thống:** `ROWCOUNT`, `TRANCOUNT`, `SPID`, `ERROR`, `SERVERNAME`, `SERVICENAME`, `VERSION`, `FETCH_STATUS`, `CURSOR_ROWS`.

---

## 3. Hướng Dẫn Cài Đặt Bản Cập Nhật 1.1.2

1. Đóng SQL Server Management Studio (SSMS).
2. Chạy file cài đặt:
   ```text
   d:\LumiSqlFormatter\LumiSqlFormatter\bin\Release\LumiSqlFormatter.vsix
   ```
3. Nhấn **Install** / **Update** để ghi đè phiên bản mới **1.1.2**.
4. Mở SSMS và trải nghiệm tính năng AutoUpperCase với hơn 510 từ khóa chuẩn T-SQL!
