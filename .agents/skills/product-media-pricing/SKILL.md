---
name: product-media-pricing
description: Cập nhật ảnh sản phẩm, giá theo khu vực, khuyến mãi và dữ liệu hiển thị của sản phẩm trong ShopApi.
---

# Giá, ảnh và hiển thị sản phẩm

## Khi dùng
Khi sửa logic hiển thị sản phẩm, ảnh sản phẩm, giá theo khu vực, khuyến mãi hoặc chi tiết sản phẩm.

## Cách làm
1. Bám theo `ProductService` khi xử lý giá cơ bản, giá theo khu vực và khuyến mãi.
2. Bám theo `ProductImageService` khi upload, xóa hoặc chọn ảnh đại diện.
3. Bám theo `ProductRegionPriceService` khi thêm/sửa/xóa giá theo vùng.
4. Giữ DTO trả về gọn, chỉ trả field cần cho frontend.

## Quy tắc
- Không phá logic `GetPriceByRegion` và `CalculatePriceWithBase`.
- Ảnh thật phải lưu trong `wwwroot/images`.
- Nếu đổi quy tắc giá hoặc ảnh, kiểm tra luôn API product detail và danh sách sản phẩm.
- Không nhét logic UI vào service.
