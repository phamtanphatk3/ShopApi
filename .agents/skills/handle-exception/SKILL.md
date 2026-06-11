---
name: handle-exception
description: Xử lý lỗi trong ShopApi theo chuẩn ApiErrorResponse. Dùng skill này khi thêm custom exception, sửa HTTP status code, chuẩn hóa response lỗi, hoặc bất kỳ lúc nào cần ném lỗi nghiệp vụ ra ngoài service — không chỉ khi sửa middleware.
---

# Xử lý lỗi cho ShopApi

## Khi dùng
Khi sửa lỗi nghiệp vụ, chuẩn hóa HTTP status code, hoặc thêm luồng bắt lỗi mới.

## Cách làm
1. Ném custom exception trong `Common/Exceptions/` cho lỗi nghiệp vụ.
2. Để `ExceptionMiddleware` bắt lỗi và trả response chuẩn.
3. Chỉ dùng `try-catch` khi thật sự cần transaction rollback hoặc cleanup.

## Quy tắc
- Response lỗi hiện tại phải đi theo format `ApiErrorResponse`.
- Không viết lỗi nghiệp vụ trực tiếp trong controller nếu có thể đẩy xuống service.
- `401` và `403` nên để middleware/JWT events xử lý, không tự bẻ trong từng action.
- Không map bừa lỗi hệ thống thành `404` hoặc `400`.
