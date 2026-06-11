---
name: shopapi-orchestrator
description: Điểm vào chính cho mọi tác vụ trong dự án ShopApi. Dùng skill này trước tiên khi làm bất kỳ việc gì với ShopApi — thêm tính năng, sửa lỗi, refactor, debug hay cấu hình — để xác định skill con phù hợp và đọc luật nền từ codex.md trước khi bắt tay vào code.
---

# ShopApi Orchestrator

## Khi dùng
Khi làm việc với dự án ShopApi và cần xác định nên dùng skill con nào trước.

## Cách dùng
1. Đọc `references/skill-map.md` trước.
2. Chọn skill con đúng theo loại việc.
3. Chỉ đọc skill con liên quan, không nạp toàn bộ skill nếu không cần.

## Quy tắc điều phối
- Auth, refresh token, logout, phân quyền → `setup-auth`
- CRUD entity, DTO, controller, service → `generate-crud`
- Validator request → `create-validator`
- Exception, status code, response lỗi → `handle-exception`
- Migration, seed, schema → `database-seed-migration`
- Chạy local, appsettings, CORS, port, startup → `local-debug-config`
- Đơn hàng, tồn kho, coupon, trạng thái đơn → `order-inventory-flow`
- Ảnh sản phẩm, giá theo khu vực, khuyến mãi hiển thị → `product-media-pricing`

## Nguyên tắc
- Không tự đoán skill nếu đã có skill phù hợp trong map.
- Nếu một task chạm nhiều mảng, ưu tiên đọc skill chính rồi mới bổ sung skill phụ.
- Giữ thay đổi nhỏ, đúng module, đúng pattern đang có của ShopApi.
