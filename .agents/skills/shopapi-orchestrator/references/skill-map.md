# Bản đồ skill ShopApi

## Mục tiêu
Giúp Codex chọn đúng skill con trước khi triển khai.

| Loại việc | Skill chính | Skill phụ nếu cần |
|---|---|---|
| Đăng nhập, đăng ký, refresh, logout, profile, role | `setup-auth` | `handle-exception` |
| Tạo CRUD mới cho entity | `generate-crud` | `create-validator`, `database-seed-migration` |
| Viết hoặc sửa validator | `create-validator` | `generate-crud` |
| Chuẩn hóa lỗi, HTTP status, response lỗi | `handle-exception` | `setup-auth` |
| Thêm/sửa migration, seed dữ liệu mẫu | `database-seed-migration` | `generate-crud` |
| Tạo đơn, hủy đơn, hoàn kho, coupon, lịch sử trạng thái | `order-inventory-flow` | `handle-exception`, `database-seed-migration` |
| Ảnh sản phẩm, giá theo khu vực, khuyến mãi hiển thị | `product-media-pricing` | `generate-crud` |

## Quy ước chọn skill
- Nếu task liên quan đến nhiều phần, đọc skill chính trước.
- Nếu chỉ cần sửa nhỏ trong một module đang có, không mở skill khác nếu chưa cần.
- Nếu task không khớp rõ ràng, ưu tiên `generate-crud` cho phần dữ liệu/API, rồi bổ sung skill phụ tương ứng.
