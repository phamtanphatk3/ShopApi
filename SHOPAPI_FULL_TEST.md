# ShopApi Full Test

Tai lieu nay chi danh cho backend.
Muc tieu la test theo dung quyen truy cap:

- **Public**: co the goi khong can token
- **User dang nhap**: can JWT
- **Admin/Staff**: can role phu hop

## 1. Smoke test

- Chay `dotnet build`
- Chay `dotnet run`
- Mo Swagger
- Kiem tra app khoi dong khong loi

## 2. Auth

### `POST /api/auth/register`

- Tao user moi
- Kiem tra username trung
- Kiem tra role hop le

### `POST /api/auth/login`

- Dang nhap dung
- Dang nhap sai mat khau
- Tra ve `token` + `user`
- Kiem tra `email`, `phone`, `address`

### `GET /api/auth/me`

- Lay profile khi da dang nhap

### `PUT /api/auth/me/password`

- Doi mat khau dung mat khau hien tai
- Doi mat khau sai current password

## 3. Products

### `GET /api/products`

- Lay danh sach san pham
- Loc theo `region`
- Phan trang / sap xep
- Co the la public neu controller duoc mo, con mac dinh van test theo JWT neu route dang bi khoa

### `GET /api/products/{id}/detail`

- Lay chi tiet san pham
- Kiem tra san pham khong ton tai

### `POST /api/products`

- Tao san pham moi
- Can quyen Admin/Staff

### `PUT /api/products/{id}`

- Cap nhat san pham
- Can quyen Admin/Staff

### `DELETE /api/products/{id}`

- Xoa san pham
- Can quyen Admin/Staff

## 4. Categories

### `GET /api/categories`

### `GET /api/categories/{id}`

### `POST /api/categories`

### `PUT /api/categories/{id}`

### `DELETE /api/categories/{id}`

- Cac API ghi du lieu nay nen test voi quyen Admin/Staff

## 5. Product images

### `GET /api/products/{productId}/images`

### `POST /api/products/{productId}/images`

- Upload anh
- Can quyen Admin/Staff

### `DELETE /api/products/{productId}/images/{imageId}`

- Xoa anh
- Can quyen Admin/Staff

### `PUT /api/products/{productId}/images/{imageId}/main`

- Dat anh dai dien
- Can quyen Admin/Staff

## 6. Inventory

### `POST /api/products/{productId}/inventory/import`

### `POST /api/products/{productId}/inventory/export`

### `GET /api/products/{productId}/inventory/history`

- `import` / `export` / `history` nen test voi quyen Staff/Admin

## 7. Region price

### `GET /api/products/{productId}/region-prices`

### `POST /api/products/{productId}/region-prices`

### `PUT /api/products/{productId}/region-prices/{region}`

### `DELETE /api/products/{productId}/region-prices/{region}`

## 8. Cart

### `POST /api/cart`

### `GET /api/cart`

### `PUT /api/cart`

### `DELETE /api/cart`

- Cac API nay phai co JWT cua user dang dang nhap

## 9. Orders

### `POST /api/orders`

- Tao don
- Kiem tra coupon
- Tru ton kho
- Phai co JWT cua customer dang dang nhap

### `GET /api/orders`

- Lay don cua customer dang dang nhap
- Phai co JWT

### `GET /api/orders/{id}`

- Lay chi tiet don
- Phai co JWT va phai dung owner/role hop le

### `PUT /api/orders/{id}/status`

- Cap nhat trang thai
- Can quyen Admin/Staff

### `PUT /api/orders/{id}/cancel`

- Huy don
- Hoan ton kho
- Hoan ma giam gia neu co
- Phai co JWT cua owner hoac role duoc phep

### `GET /api/orders/all`

- Admin/Staff lay tat ca don
- Can quyen Admin/Staff

## 10. Coupons

### `GET /api/coupons`

### `GET /api/coupons/{id}`

### `POST /api/coupons`

### `PUT /api/coupons/{id}`

### `DELETE /api/coupons/{id}`

### `GET /api/coupons/validate`

- Kiem tra coupon hop le
- `validate` co the duoc frontend goi khi customer checkout
- Cac API CRUD coupon can quyen Admin/Staff

## 11. Promotions

### `GET /api/promotions`

### `GET /api/promotions/{id}`

### `POST /api/promotions`

### `PUT /api/promotions/{id}`

### `DELETE /api/promotions/{id}`

### `POST /api/promotions/{promoId}/products/{productId}`

### `DELETE /api/promotions/{promoId}/products/{productId}`

- Cac API CRUD/gia gan promotion can quyen Admin/Staff

## 12. Stores

### `GET /api/stores`

### `GET /api/stores/{id}`

### `POST /api/stores`

### `PUT /api/stores/{id}`

### `DELETE /api/stores/{id}`

### `GET /api/stores/by-province`

### `GET /api/stores/nearest`

### `GET /api/stores/has-product`

- Cac API CRUD store can quyen Admin/Staff
- Cac API truy van store co the duoc frontend goi khi can hien thi diem ban hang

## 13. Warranty

### `POST /api/warranty`

### `GET /api/warranty/lookup`

- `lookup` co the la public hoac co JWT tuy controller hien tai

## 14. Installments

### `POST /api/installments`

- Thuong la user dang dang nhap tao yeu cau

## 15. Reports

### `GET /api/reports/revenue/daily`

### `GET /api/reports/revenue/monthly`

### `GET /api/reports/top-selling-products`

### `GET /api/reports/orders-by-status`

### `GET /api/reports/low-stock`

- Nhom API bao cao nay can quyen Admin/Staff

## 16. Wishlist

### `GET /api/wishlist`

### `POST /api/wishlist/{productId}`

### `DELETE /api/wishlist/{productId}`

- Can JWT cua user dang dang nhap

## 17. Kiem tra response

Kiem tra:

- `200` cho thanh cong
- `400` cho validation / business rule
- `401` cho chua dang nhap
- `403` cho khong du quyen
- `404` cho khong tim thay

## 18. Du lieu mau

Sau khi chay `dotnet run` trong Development, app se:

- migrate database
- seed du lieu mau neu bang san pham dang trong

Tai khoan mau:

- `admin / 123`
- `staff / 123`
- `customer / 123`

## 19. Cach doc file nay cho dung

- Neu la user/frontend: chi test cac API can JWT cua user va cac API public.
- Neu la staff/admin: test them cac API CRUD, inventory, report, promotion, coupon.
- Neu thay `401` thi kiem tra token.
- Neu thay `403` thi kiem tra role.
