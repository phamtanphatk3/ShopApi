# ShopApi Full Test

Tai lieu nay dung de test nhanh backend theo tung nhom quyen.

## 1. Quy tac chung

- Public: goi khong can token
- User: can JWT
- Admin/Staff: can JWT va role phu hop

## 2. Smoke test

- Chay `dotnet build`
- Chay `dotnet run`
- Mo Swagger
- Kiem tra app khoi dong binh thuong

## 3. Auth

- `POST /api/auth/register` - tao user moi
- `POST /api/auth/login` - tra `token` + `user`
- `GET /api/auth/me` - lay profile
- `PUT /api/auth/me/password` - doi mat khau

## 4. San pham va danh muc

- `GET /api/products` - danh sach san pham
- `GET /api/products/{id}/detail` - chi tiet san pham
- `POST /api/products` - tao san pham
- `PUT /api/products/{id}` - cap nhat san pham
- `DELETE /api/products/{id}` - xoa san pham
- `GET /api/categories` / `GET /api/categories/{id}` - danh muc
- `POST/PUT/DELETE /api/categories` - CRUD danh muc

## 5. Anh, gia khu vuc, ton kho

- `GET/POST/DELETE /api/products/{productId}/images` - anh san pham
- `PUT /api/products/{productId}/images/{imageId}/main` - dat anh dai dien
- `GET/POST/PUT/DELETE /api/products/{productId}/region-prices` - gia theo khu vuc
- `POST /api/products/{productId}/inventory/import` - nhap kho
- `POST /api/products/{productId}/inventory/export` - xuat kho
- `GET /api/products/{productId}/inventory/history` - lich su ton kho

## 6. Gio hang, don hang, yeu thich

- `POST/GET/PUT/DELETE /api/cart` - gio hang
- `POST /api/orders` - tao don
- `GET /api/orders` - don cua customer dang dang nhap
- `GET /api/orders/{id}` - chi tiet don
- `PUT /api/orders/{id}/status` - cap nhat trang thai
- `PUT /api/orders/{id}/cancel` - huy don
- `GET /api/orders/all` - admin/staff lay tat ca don
- `GET/POST/DELETE /api/wishlist` - danh sach yeu thich

## 7. Coupon, khuyen mai, cua hang

- `GET/POST/PUT/DELETE /api/coupons` - CRUD coupon
- `GET /api/coupons/validate` - kiem tra coupon
- `GET/POST/PUT/DELETE /api/promotions` - CRUD khuyen mai
- `POST/DELETE /api/promotions/{promoId}/products/{productId}` - gan/bo san pham
- `GET/POST/PUT/DELETE /api/stores` - CRUD cua hang
- `GET /api/stores/by-province`
- `GET /api/stores/nearest`
- `GET /api/stores/has-product`

## 8. Bao hanh, tra gop, bao cao

- `POST /api/warranty` - tao bao hanh
- `GET /api/warranty/lookup` - tra cuu bao hanh
- `POST /api/installments` - tao yeu cau tra gop
- `GET /api/reports/revenue/daily`
- `GET /api/reports/revenue/monthly`
- `GET /api/reports/top-selling-products`
- `GET /api/reports/orders-by-status`
- `GET /api/reports/low-stock`

## 9. Muc tieu can test

- `200` cho thanh cong
- `400` cho validation / business rule
- `401` cho chua dang nhap
- `403` cho khong du quyen
- `404` cho khong tim thay

## 10. Du lieu mau

Khi chay Development, app co the seed:

- `admin / 123`
- `staff / 123`
- `customer / 123`
- `WELCOME10`
- `SHIPFREE`

## 11. Cach doc nhanh

- Frontend: test public + user APIs
- Staff/Admin: test them CRUD, inventory, report, promotion, coupon
- Neu gap `401` thi kiem tra token
- Neu gap `403` thi kiem tra role
