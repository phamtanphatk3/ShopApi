---
name: local-debug-config
description: Cấu hình và debug môi trường local cho ShopApi. Dùng skill này ngay khi app không lên được, lỗi build, lỗi CORS, lỗi connection string, sai port, hay bất kỳ vấn đề nào xảy ra trước khi request đầu tiên chạy được — đừng đụng code nghiệp vụ trước khi xem xong skill này.
---

# Cấu hình chạy local cho ShopApi

## Khi dùng
Khi cần chạy dự án trên máy local, sửa cấu hình môi trường, hoặc xử lý lỗi build/debug/startup.

## Cách làm
1. Kiểm tra `appsettings.json`, `appsettings.Development.json`, `appsettings.Production.json`.
2. Đối chiếu connection string với DB provider đang dùng trong `Program.cs` và `AppDbContextFactory.cs`.
3. Kiểm tra CORS, JWT key, port chạy, và profile debug của Visual Studio.
4. Nếu app không lên, đọc log khởi động và xử lý theo lỗi gốc trước.

## Quy tắc
- Không sửa cấu hình theo cảm tính; phải khớp với provider thật của repo.
- Không để secret thật trong file config nếu dự án đã có cách khác an toàn hơn.
- Nếu lỗi liên quan port, process lock, hoặc `dotnet run`, ưu tiên kiểm tra môi trường local trước khi đụng code nghiệp vụ.
- Luôn giữ thay đổi nhỏ và chỉ sửa đúng chỗ cần để app chạy được.
