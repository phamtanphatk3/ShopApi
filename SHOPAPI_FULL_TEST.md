# ShopApi Full Test

Tai lieu nay chi danh cho backend.

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

### `GET /api/products/{id}/detail`

- Lay chi tiet san pham
- Kiem tra san pham khong ton tai

### `POST /api/products`

- Tao san pham moi

### `PUT /api/products/{id}`

- Cap nhat san pham

### `DELETE /api/products/{id}`

- Xoa san pham

## 4. Categories

### `GET /api/categories`

### `GET /api/categories/{id}`

### `POST /api/categories`

### `PUT /api/categories/{id}`

### `DELETE /api/categories/{id}`

## 5. Product images

### `GET /api/products/{productId}/images`

### `POST /api/products/{productId}/images`

- Upload anh

### `DELETE /api/products/{productId}/images/{imageId}`

- Xoa anh

### `PUT /api/products/{productId}/images/{imageId}/main`

- Dat anh dai dien

## 6. Inventory

### `POST /api/products/{productId}/inventory/import`

### `POST /api/products/{productId}/inventory/export`

### `GET /api/products/{productId}/inventory/history`

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

## 9. Orders

### `POST /api/orders`

- Tao don
- Kiem tra coupon
- Tru ton kho

### `GET /api/orders`

- Lay don cua customer dang dang nhap

### `GET /api/orders/{id}`

- Lay chi tiet don

### `PUT /api/orders/{id}/status`

- Cap nhat trang thai

### `PUT /api/orders/{id}/cancel`

- Huy don
- Hoan ton kho
- Hoan ma giam gia neu co

### `GET /api/orders/all`

- Admin/Staff lay tat ca don

## 10. Coupons

### `GET /api/coupons`

### `GET /api/coupons/{id}`

### `POST /api/coupons`

### `PUT /api/coupons/{id}`

### `DELETE /api/coupons/{id}`

### `GET /api/coupons/validate`

- Kiem tra coupon hop le

## 11. Promotions

### `GET /api/promotions`

### `GET /api/promotions/{id}`

### `POST /api/promotions`

### `PUT /api/promotions/{id}`

### `DELETE /api/promotions/{id}`

### `POST /api/promotions/{promoId}/products/{productId}`

### `DELETE /api/promotions/{promoId}/products/{productId}`

## 12. Stores

### `GET /api/stores`

### `GET /api/stores/{id}`

### `POST /api/stores`

### `PUT /api/stores/{id}`

### `DELETE /api/stores/{id}`

### `GET /api/stores/by-province`

### `GET /api/stores/nearest`

### `GET /api/stores/has-product`

## 13. Warranty

### `POST /api/warranty`

### `GET /api/warranty/lookup`

## 14. Installments

### `POST /api/installments`

## 15. Reports

### `GET /api/reports/revenue/daily`

### `GET /api/reports/revenue/monthly`

### `GET /api/reports/top-selling-products`

### `GET /api/reports/orders-by-status`

### `GET /api/reports/low-stock`

## 16. Wishlist

### `GET /api/wishlist`

### `POST /api/wishlist/{productId}`

### `DELETE /api/wishlist/{productId}`

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
