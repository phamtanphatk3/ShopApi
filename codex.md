# ShopApi Codex

## Mục đích
ShopApi là ASP.NET Core Web API cho hệ thống bán hàng thiết bị điện tử. File này là luật chung của dự án, không phải tài liệu hướng dẫn người dùng.

## Điều phối mặc định
- Khi làm việc với ShopApi, mặc định gọi `shopapi-orchestrator` trước.
- Chỉ gọi skill con trực tiếp khi task đã rõ ràng và chỉ chạm một mảng cụ thể.
- `shopapi-orchestrator` sẽ dẫn tới skill phù hợp trong `.agents/skills/`.

## Luật cốt lõi
- Ưu tiên đọc code hiện có trước khi sửa.
- Giữ async-first cho mọi thao tác I/O.
- Validation đặt trong `Validators/` bằng FluentValidation.
- Lỗi nghiệp vụ đi qua custom exception và `ExceptionMiddleware`.
- Auth mặc định dùng JWT, `FallbackPolicy` khóa endpoint nếu không có `[AllowAnonymous]`.
- Lỗi chạy local, appsettings, CORS, port, startup thì ưu tiên `local-debug-config`.
- Không tự thêm pattern không có trong repo.
- Không sửa file sinh tự động hoặc thư mục build như `bin/`, `obj/`, `.vs/`.    

## Phạm vi làm việc
- Controller chỉ xử lý HTTP.
- Service giữ business logic.
- DbContext, model và migration phải đi cùng nhau khi đổi schema.
- Tài liệu kỹ thuật phải khớp đúng với cấu trúc hiện tại của repo.

## Quan hệ với `.agents/skills/`
- `codex.md` giữ luật nền của dự án.
- `.agents/skills/` giữ luật chuyên môn theo từng mảng.
- Nếu task chạm nhiều mảng, để `shopapi-orchestrator` phân công trước rồi mới mở skill con cần thiết.
