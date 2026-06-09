---
name: generate-crud
description: Tạo nhanh CRUD chuẩn ShopApi cho entity mới: DTO, validator, service, controller và cập nhật model/DbContext/migration khi cần.
---

# Generate CRUD cho ShopApi

## Khi dùng
Khi người dùng yêu cầu thêm mới hoặc mở rộng CRUD cho một entity trong ShopApi.

## Cách làm
1. Đọc pattern hiện có của đúng module đó trước.
2. Tạo DTO trong `DTOs/<Domain>/` theo kiểu `CreateDto`, `UpdateDto`, `ResponseDto`.
3. Tạo validator trong `Validators/` theo đúng cách dự án đang nhóm validator.
4. Tạo service trong `Services/`.
   - Ưu tiên dùng `AppDbContext` trực tiếp nếu module hiện tại đang dùng kiểu này.
   - Chỉ dùng repository/interface nếu domain đó đã có sẵn pattern này.
5. Tạo controller trong `Controllers/` kế thừa `ControllerBase`.
6. Nếu thay đổi schema, cập nhật `Models/`, `Data/AppDbContext.cs` và tạo migration mới.

## Quy tắc bắt buộc
- Không tự thêm `BaseController`, `Services/Implementations` hoặc `IMapper` nếu dự án không có pattern đó.
- Controller chỉ xử lý HTTP.
- Logic nghiệp vụ nằm trong service.
- Validate bằng FluentValidation.
- Lỗi nghiệp vụ phải đi qua custom exception và `ExceptionMiddleware`.
