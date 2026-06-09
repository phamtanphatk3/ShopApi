---
name: database-seed-migration
description: Cập nhật schema, migration và dữ liệu seed development cho ShopApi.
---

# Migration và seed dữ liệu

## Khi dùng
Khi thêm/sửa model, cần migration mới, hoặc cần bổ sung dữ liệu mẫu để chạy local.

## Cách làm
1. Cập nhật model và `Data/AppDbContext.cs` trước.
2. Tạo migration mới, không sửa migration cũ để “làm đẹp”.
3. Chạy database update sau khi migration đã ổn.
4. Seed dữ liệu development trong `DbSeeder` theo kiểu an toàn, không ghi đè dữ liệu thật.

## Quy tắc
- Dữ liệu seed phải idempotent nếu có thể.
- Nếu bảng đã có dữ liệu thật thì không seed lại bừa.
- Bám theo flow hiện có của dự án: seed chỉ chạy ở Development.
- Khi thêm seed mới, kiểm tra luôn unique index và quan hệ khóa ngoại.
