---
name: create-validator
description: Tạo và cập nhật validator FluentValidation cho ShopApi. Dùng skill này khi thêm DTO mới, sửa rule validation, thêm field vào request hiện có, hoặc chuẩn hóa error message — kể cả khi task nhỏ chỉ là thêm một rule vào validator đang có.
---

# Tạo validator cho ShopApi

## Khi dùng
Khi thêm DTO mới hoặc cần siết lại validation cho một request hiện có.

## Cách làm
1. Đặt validator trong `Validators/`, theo kiểu file đang có sẵn trong dự án.
2. Kế thừa `AbstractValidator<T>`.
3. Viết rule trong constructor, ưu tiên rõ ràng và ngắn gọn.
4. Đăng ký bằng quét assembly nếu validator nằm cùng project.

## Quy tắc
- Bám theo cấu trúc hiện tại: dự án đang dùng các file như `AuthValidators.cs`, `ProductValidators.cs`, `CommerceValidators.cs`.
- Không tách thành nhiều lớp lạ nếu cùng domain đang được gom chung trong một file.
- Message nên ngắn, rõ, nhất quán với tiếng Việt của API.
- Chỉ sửa `Program.cs` nếu thật sự cần thêm assembly mới; hiện tại dự án đã đăng ký validator tự động.
