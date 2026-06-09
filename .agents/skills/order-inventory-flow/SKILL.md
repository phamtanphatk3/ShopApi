---
name: order-inventory-flow
description: Xử lý luồng tạo đơn, hủy đơn, hoàn kho, coupon và lịch sử trạng thái trong ShopApi.
---

# Luồng đơn hàng và tồn kho

## Khi dùng
Khi sửa checkout, cancel order, cập nhật trạng thái đơn, tồn kho, hoặc coupon.

## Cách làm
1. Khi tạo đơn: kiểm tra giỏ hàng, kiểm tra tồn kho, tạo order items, trừ kho, ghi lịch sử trạng thái, xử lý coupon.
2. Khi hủy đơn: chỉ cho phép các trạng thái hợp lệ, hoàn kho, giảm lại usage của coupon nếu có, ghi lý do hủy.
3. Khi đổi trạng thái: luôn ghi vào bảng lịch sử trạng thái đơn hàng.
4. Mọi thay đổi phải chạy trong transaction nếu có nhiều bước liên quan.

## Quy tắc
- `Customer` chỉ thao tác trên đơn của mình.
- `Admin/Staff` có thể quản lý đơn theo role đã cấu hình.
- Không cập nhật stock rời rạc ngoài service xử lý đơn/tồn kho.
- Không quên đồng bộ `Order`, `InventoryTransaction`, `Coupon` và `OrderStatusHistory`.
