---
name: setup-auth
description: Thiết lập và cập nhật JWT, refresh token, logout, profile và phân quyền trong ShopApi.
---

# Thiết lập auth cho ShopApi

## Khi dùng
Khi sửa luồng đăng nhập, đăng ký, refresh token, logout, profile, hoặc phân quyền endpoint.

## Cách làm
1. Giữ JWT Bearer là cơ chế chính.
2. Giữ `FallbackPolicy` để endpoint mặc định yêu cầu đăng nhập.
3. Dùng `[AllowAnonymous]` cho endpoint public.
4. Dùng `[Authorize(Roles = "Admin,Staff")]` cho endpoint quản trị khi cần.
5. Refresh token phải được lưu trong DB và có trạng thái revoke/expire.

## Quy tắc
- Login của ShopApi trả `accessToken`, `refreshToken`, `refreshTokenExpiresAt` và `user`.
- Logout/refresh phải revoke token cũ nếu luồng đó đã được dùng.
- Nếu cần thêm field người dùng, sửa luôn `User`, DTO profile và response auth.
- Không để secret auth nằm cứng trong code.
