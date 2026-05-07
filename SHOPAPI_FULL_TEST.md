# SHOPAPI_FULL_TEST.md

## 0) Thong tin chung
Base URL:
- `https://localhost:7212`

Header dung chung:
- `Content-Type: application/json` (voi API JSON)
- `Authorization: Bearer <token>` (tru endpoint AllowAnonymous)

Tai khoan mau:
- Admin: `admin / 123`
- Customer: `user1 / 123`

---

## 1) Auth

### 1.1 Dang ky tai khoan
- API: `POST /api/auth/register`
- Muc dich: tao user moi (mac dinh role Customer neu khong truyen role)
- Quyen: AllowAnonymous
- Body:
```json
{
  "username": "customer_test_01",
  "password": "123",
  "role": "Customer"
}
```

### 1.2 Dang nhap
- API: `POST /api/auth/login`
- Muc dich: lay JWT token
- Quyen: AllowAnonymous
- Body:
```json
{
  "username": "admin",
  "password": "123"
}
```

---

## 2) Categories (Bai 1)

### 2.1 Tao category cha
- API: `POST /api/categories`
- Muc dich: tao danh muc cha
- Quyen: Admin/Staff
- Body:
```json
{
  "name": "REP0507 Dien Thoai",
  "slug": "rep0507-dien-thoai",
  "parentCategoryId": null
}
```

### 2.2 Tao category con
- API: `POST /api/categories`
- Muc dich: tao danh muc con
- Quyen: Admin/Staff
- Body (doi `parentCategoryId` theo id category cha vua tao):
```json
{
  "name": "REP0507 Samsung",
  "slug": "rep0507-samsung",
  "parentCategoryId": 1
}
```

### 2.3 Lay danh sach category
- API: `GET /api/categories`
- Muc dich: kiem tra danh sach category
- Quyen: co the public

### 2.4 Lay chi tiet category
- API: `GET /api/categories/{id}`
- Muc dich: xem thong tin category theo id
- Quyen: co the public

### 2.5 Cap nhat category
- API: `PUT /api/categories/{id}`
- Muc dich: sua category
- Quyen: Admin/Staff
- Body:
```json
{
  "name": "REP0507 Samsung Updated",
  "slug": "rep0507-samsung-updated",
  "parentCategoryId": 1
}
```

### 2.6 Xoa mem category
- API: `DELETE /api/categories/{id}`
- Muc dich: xoa mem category
- Quyen: Admin/Staff
- Luu y: category dang co product se bi chan

---

## 3) Products (Bai 2 + Bai 4)

### 3.1 Tao san pham
- API: `POST /api/products`
- Muc dich: tao product
- Quyen: Admin/Staff
- Body:
```json
{
  "name": "REP0507 Galaxy A",
  "sku": "REP0507-GA-001",
  "brand": "Samsung",
  "price": 23000000,
  "stockQuantity": 10,
  "description": "San pham test",
  "categoryId": 1
}
```

### 3.2 Lay danh sach san pham (listing)
- API: `GET /api/products`
- Muc dich: listing + filter + sort + paging
- Quyen: public
- Query mau:
```text
?keyword=Galaxy&brand=Samsung&minPrice=10000000&maxPrice=30000000&sortBy=price_desc&pageNumber=1&pageSize=10
```

### 3.3 Lay chi tiet san pham
- API: `GET /api/products/{id}/detail`
- Muc dich: xem detail + image + specs + related + finalPrice
- Quyen: public

### 3.4 Cap nhat san pham
- API: `PUT /api/products/{id}`
- Muc dich: sua product
- Quyen: Admin/Staff
- Body:
```json
{
  "name": "REP0507 Galaxy A Updated",
  "sku": "REP0507-GA-001",
  "brand": "Samsung",
  "price": 22500000,
  "stockQuantity": 12,
  "description": "Updated",
  "categoryId": 1
}
```

### 3.5 Xoa san pham
- API: `DELETE /api/products/{id}`
- Muc dich: xoa mem product
- Quyen: Admin/Staff

---

## 4) Product Images (Bai 3)

### 4.1 Upload anh
- API: `POST /api/products/{productId}/images`
- Muc dich: them anh, dat anh dai dien, sap xep thu tu
- Quyen: Admin/Staff
- Form-data:
- `File`: (chon file)
- `IsMain`: `true`
- `SortOrder`: `1`

### 4.2 Lay danh sach anh
- API: `GET /api/products/{productId}/images`
- Muc dich: xem danh sach anh cua product

---

## 5) Promotions (Bai 5)

### 5.1 Tao promotion
- API: `POST /api/promotions`
- Muc dich: tao khuyen mai
- Quyen: Admin/Staff
- Body:
```json
{
  "name": "SALE20",
  "discountType": "Percent",
  "discountValue": 20,
  "startDate": "2026-05-01T00:00:00",
  "endDate": "2026-12-31T23:59:59"
}
```

### 5.2 Gan promotion vao product
- API: `POST /api/promotions/{promoId}/products/{productId}`
- Muc dich: map khuyen mai vao san pham
- Quyen: Admin/Staff

---

## 6) Product Specifications (Bai 6)
- Muc dich nghiep vu: thong so ky thuat trong detail.
- Kiem tra qua:
- `GET /api/products/{id}/detail`
- Xem `specifications` co du lieu.

---

## 7) Inventory (bo tro Bai 8/11/16)

### 7.1 Nhap kho
- API: `POST /api/products/{productId}/inventory/import`
- Muc dich: tang ton
- Quyen: Admin/Staff
- Body:
```json
{
  "quantity": 20,
  "note": "Nhap kho test"
}
```

### 7.2 Xuat kho
- API: `POST /api/products/{productId}/inventory/export`
- Muc dich: giam ton
- Quyen: Admin/Staff
- Body:
```json
{
  "quantity": 2,
  "note": "Xuat kho test"
}
```

### 7.3 Lich su kho
- API: `GET /api/products/{productId}/inventory/history`
- Muc dich: xem lich su giao dich kho

---

## 8) Cart (Bai 7)

### 8.1 Them vao gio
- API: `POST /api/cart`
- Muc dich: add cart item
- Quyen: Customer
- Body:
```json
{
  "productId": 1,
  "quantity": 2
}
```

### 8.2 Xem gio hang
- API: `GET /api/cart`
- Muc dich: xem item + tong tien
- Quyen: Customer

### 8.3 Cap nhat so luong
- API: `PUT /api/cart?itemId={itemId}&quantity=3`
- Muc dich: doi quantity item
- Quyen: Customer

### 8.4 Xoa item
- API: `DELETE /api/cart?itemId={itemId}`
- Muc dich: xoa item khoi gio
- Quyen: Customer

---

## 9) Orders (Bai 8)

### 9.1 Tao don tu gio hang
- API: `POST /api/orders`
- Muc dich: tao order, check ton kho, tru ton kho
- Quyen: Customer
- Body:
```json
{
  "couponCode": "SALE10"
}
```

### 9.2 Xem don cua toi
- API: `GET /api/orders`
- Muc dich: customer xem don cua chinh minh
- Quyen: Customer

### 9.3 Xem chi tiet don
- API: `GET /api/orders/{id}`
- Muc dich: xem detail order
- Quyen: owner hoac Admin/Staff

### 9.4 Admin xem tat ca don
- API: `GET /api/orders/all`
- Muc dich: danh sach tat ca order
- Quyen: Admin/Staff

### 9.5 Cap nhat trang thai don
- API: `PUT /api/orders/{id}/status?status=Confirmed`
- Muc dich: doi trang thai order
- Quyen: Admin/Staff

---

## 10) Coupon (Bai 9)

### 10.1 Validate coupon rieng
- API: `GET /api/coupons/validate?code=SALE10&orderAmount=50000000`
- Muc dich: kiem tra coupon truoc khi dat hang
- Quyen: da dang nhap

---

## 11) Region Prices (Bai 10)

### 11.1 Tao gia theo khu vuc
- API: `POST /api/products/{productId}/region-prices`
- Muc dich: them gia rieng theo tinh/thanh
- Quyen: Admin/Staff
- Body:
```json
{
  "region": "HCM",
  "price": 21000000
}
```

### 11.2 Sua gia khu vuc
- API: `PUT /api/products/{productId}/region-prices/{region}`
- Muc dich: cap nhat gia vung
- Quyen: Admin/Staff
- Body:
```json
{
  "price": 20500000
}
```

### 11.3 Xoa gia khu vuc
- API: `DELETE /api/products/{productId}/region-prices/{region}`
- Muc dich: fallback ve gia mac dinh
- Quyen: Admin/Staff

### 11.4 Xem gia khu vuc
- API: `GET /api/products/{productId}/region-prices`
- Muc dich: danh sach gia theo vung

---

## 12) Stores (Bai 11)

### 12.1 Tim theo tinh
- API: `GET /api/stores/by-province?province=HCM`
- Muc dich: list store theo tinh

### 12.2 Tim gan nhat
- API: `GET /api/stores/nearest?lat=10.85&lng=106.77`
- Muc dich: tim store gan toa do

### 12.3 Kiem tra con hang
- API: `GET /api/stores/has-product?productId=1`
- Muc dich: store nao con ton san pham

---

## 13) Warranty (Bai 12)

### 13.1 Tao bao hanh
- API: `POST /api/warranty`
- Muc dich: tao record bao hanh
- Quyen: Admin/Staff
- Body:
```json
{
  "serialNumber": "REP0507-SN-001",
  "customerPhone": "0909000001",
  "orderId": 1,
  "productId": 1,
  "warrantyMonths": 12
}
```

### 13.2 Tra cuu bao hanh
- API: `GET /api/warranty/lookup`
- Muc dich: tra cuu theo serial/phone/orderId
- Quyen: dang nhap
- Query mau:
```text
?serial=REP0507-SN-001
```
hoac
```text
?phone=0909000001
```
hoac
```text
?orderId=1
```

---

## 14) Installments (Bai 13)

### 14.1 Mo phong tra gop
- API: `POST /api/installments`
- Muc dich: tinh tra truoc + tra gop theo ky han
- Body:
```json
{
  "productId": 1,
  "fullName": "Nguyen Van A",
  "phone": "0909111222",
  "months": 12,
  "downPaymentPercent": 30
}
```

---

## 15) Reports (Bai 16)

### 15.1 Doanh thu theo ngay
- API: `GET /api/reports/revenue/daily?from=2026-05-01&to=2026-05-31`
- Quyen: Admin/Staff

### 15.2 Doanh thu theo thang
- API: `GET /api/reports/revenue/monthly?from=2026-01-01&to=2026-12-31`
- Quyen: Admin/Staff

### 15.3 Top ban chay
- API: `GET /api/reports/top-selling-products?top=5`
- Quyen: Admin/Staff

### 15.4 Don theo trang thai
- API: `GET /api/reports/orders-by-status`
- Quyen: Admin/Staff

### 15.5 Ton kho thap
- API: `GET /api/reports/low-stock?threshold=5`
- Quyen: Admin/Staff

---

## 16) Test role nhanh (Bai 14)

### 16.1 Customer khong duoc CRUD product
- Login `user1` -> goi `POST /api/products`
- Ky vong: `403`

### 16.2 Admin duoc CRUD product
- Login `admin` -> goi `POST /api/products`
- Ky vong: `200`

### 16.3 Khong token
- Goi `POST /api/orders`
- Ky vong: `401`

---

## 17) Loi hay gap
- `401`: token sai/het han/thieu Bearer
- `403`: dung token nhung sai role
- `400`: body khong hop le / vi pham validation
- `404`: id khong ton tai

