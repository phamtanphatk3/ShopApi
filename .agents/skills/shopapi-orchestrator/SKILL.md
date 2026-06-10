---
name: shopapi-orchestrator
description: Điều phối mọi tác vụ trong ShopApi, xác định skill con phù hợp và hướng Codex đọc đúng phần cần thiết trước khi làm việc.
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
- Đơn hàng, tồn kho, coupon, trạng thái đơn → `order-inventory-flow`
- Ảnh sản phẩm, giá theo khu vực, khuyến mãi hiển thị → `product-media-pricing`

## Nguyên tắc
- Không tự đoán skill nếu đã có skill phù hợp trong map.
- Nếu một task chạm nhiều mảng, ưu tiên đọc skill chính rồi mới bổ sung skill phụ.
- Giữ thay đổi nhỏ, đúng module, đúng pattern đang có của ShopApi.
