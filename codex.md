# codex.md

## 1. Tổng quan & Kiến trúc
- **Project:** ShopApi (.NET 10.0 Web API).
- **Kiến trúc:** Clean Architecture (Client -> Controller -> FluentValidation -> Service -> Repository -> SQL Server).
- **Nguyên tắc cốt lõi:** Luôn ưu tiên Async, tách biệt mối quan tâm, bảo mật theo vai trò (Role-based).

## 2. Quy tắc Code (Skills)
- **Async-First:** Mọi method I/O bắt buộc có hậu tố `Async` (ex: `GetProductByIdAsync`). Sử dụng đúng EF Core Async methods.
- **Tối ưu truy vấn:** Sử dụng `Include/ThenInclude` để nạp dữ liệu liên quan. Cấm tuyệt đối Lazy Loading.
- **Xử lý lỗi:** Logic nghiệp vụ dùng Custom Exception trong `Common/Exceptions/`. Tất cả lỗi được bọc bởi `ExceptionMiddleware`.
- **Naming:** Interface bắt đầu bằng `I` (ex: `IProductService`). DTO không chứa logic, không expose Entity.

## 3. Rào chắn (Guardrails)
- **Phạm vi:** Chỉ sửa file trong phạm vi yêu cầu. Không tự ý thay đổi route, contract API hoặc DB Schema.
- **An toàn:** Không cài đặt package lạ khi chưa được duyệt. Không can thiệp vào `bin/`, `obj/`, `.vs/`, hoặc các file Migration cũ.
- **Bảo mật:** Không để lộ secret, API key hoặc URL cứng trong code.

## 4. Quy trình vận hành & MCP (Agentic Workflows)
AI Agent phải sử dụng các bộ công cụ trong thư mục `.system/` để đảm bảo tính nhất quán:

* **Changelog Generator:** Mọi thay đổi lớn phải được ghi lại tại `changelog-generator`.
* **Codebase Migrate:** Mọi thay đổi Database Schema phải đi qua workflow tại `codebase-migrate`.
* **Issue Triage:** Phân tích lỗi dựa trên `issue-triage` trước khi fix.
* **Meeting & Actions:** Các yêu cầu từ `meeting-notes-and-actions` là nguồn ưu tiên cho các task mới.
* **PR Review & CI Fix:** Bắt buộc chạy qua `pr-review-ci-fix` để kiểm tra lỗi CI trước khi hoàn tất commit.
* **Sentry Triage:** Đối chiếu lỗi thực tế tại `sentry-triage` trước khi thực hiện Debug.

## 5. Thực thi & Xác nhận
1. **Lead Agent** tiếp nhận yêu cầu và phân tích dựa trên file này.
2. **Subagents** triển khai code theo tầng (Backend, Validation, Database).
3. **Tự động hóa:** Sau khi code xong, Agent **phải** chạy `dotnet build` và lệnh `migration` tương ứng qua giao thức MCP.
4. **Tự sửa lỗi:** Nếu `dotnet build` thất bại, Agent tự động kích hoạt kỹ năng Debug, đọc log và tự sửa cho đến khi Build/Test thành công.