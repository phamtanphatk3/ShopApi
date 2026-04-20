# SHOPAPI_TEST_1_16.md

## 0) Chuan bi chung

### 0.1 Chay backend
- Profile: `https`
- URL khuyen nghi local: `https://localhost:7212`
- Neu test LAN: `https://<ip-may-ban>:7212`

### 0.2 Dang nhap lay token
- `POST /api/auth/login`
- Body admin:
```json
{
  "username": "admin",
  "password": "123"
}
```
- Body customer:
```json
{
  "username": "user1",
  "password": "123"
}
```
- Header dung cho API can auth:
```text
Authorization: Bearer <TOKEN>
```

### 0.3 Du lieu mau toi thieu
- Category goc: Dien thoai
- 2 san pham cung category de test related
- Co ton kho > 0 de test cart/order

---

## 1) Bai 1 - CRUD danh muc

### Endpoint
- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories` (Admin,Staff)
- `PUT /api/categories/{id}` (Admin,Staff)
- `DELETE /api/categories/{id}` (Admin,Staff, xoa mem)

### Body mau
`POST /api/categories`
```json
{
  "name": "Dien thoai Samsung",
  "parentCategoryId": 1
}
```

`PUT /api/categories/{id}`
```json
{
  "name": "Dien thoai Samsung Update",
  "isActive": true
}
```

### Kiem tra
- Tao danh muc con co `parentCategoryId` => PASS parent-child.
- Xoa danh muc dang co san pham => bi chan (message: cannot delete...).
- Xoa danh muc khong co san pham => `isActive=false` (xoa mem).

---

## 2) Bai 2 - CRUD san pham

### Endpoint
- `GET /api/products`
- `POST /api/products` (Admin,Staff)
- `PUT /api/products/{id}` (Admin,Staff)
- `DELETE /api/products/{id}` (Admin,Staff)

### Body mau tao
```json
{
  "name": "Samsung Galaxy S31",
  "sku": "SS-S31",
  "brand": "Samsung",
  "price": 25500000,
  "stockQuantity": 10,
  "description": "Flagship",
  "categoryId": 1
}
```

### Query test
- Search: `/api/products?keyword=S31`
- Search SKU: `/api/products?keyword=SS-S31`
- Filter category: `/api/products?categoryId=1`
- Filter brand: `/api/products?brand=Samsung`
- Sort: `/api/products?sortBy=price_asc` va `price_desc`
- Paging: `/api/products?pageNumber=1&pageSize=5`

### Kiem tra
- Tao SKU trung => loi.
- Danh sach tra `items,totalCount,totalPages,currentPage`.

---

## 3) Bai 3 - Anh san pham

### Endpoint
- `POST /api/products/{productId}/images` (Admin,Staff)
- `GET /api/products/{productId}/images`

### Form-data mau
- `File`: chon file anh
- `IsPrimary`: `true`
- `SortOrder`: `1`

### Kiem tra
- Upload nhieu anh cho cung 1 product.
- Upload 2 anh voi `SortOrder` khac nhau, goi GET kiem tra thu tu.
- Co the danh dau anh dai dien (`IsPrimary=true`).

---

## 4) Bai 4 - Listing API

### Endpoint
- `GET /api/products`

### Query day du
`/api/products?keyword=samsung&categoryId=1&brand=Samsung&minPrice=10000000&maxPrice=30000000&sortBy=price_desc&pageNumber=1&pageSize=10`

### Kiem tra
- Dung du tham so va response dung format pagination.

---

## 5) Bai 5 - Khuyen mai + gia sau giam

### Endpoint
- `POST /api/promotions` (Admin,Staff)
- `POST /api/promotions/{promoId}/products/{productId}` (Admin,Staff)
- Kiem tra gia sau giam tai:
  - `GET /api/products`
  - `GET /api/products/{id}/detail`

### Tao promo %
```json
{
  "name": "SALE20",
  "discountPercent": 20,
  "discountAmount": null
}
```

### Tao promo tien co dinh
```json
{
  "name": "GIAM500K",
  "discountPercent": null,
  "discountAmount": 500000
}
```

### Kiem tra
- Gan promo vao product.
- `finalPrice` thay doi dung tren list va detail.

---

## 6) Bai 6 - Chi tiet san pham + thong so + related

### Endpoint hien co
- `GET /api/products/{id}/detail`

### Tao thong so ky thuat (SQL)
```sql
INSERT INTO ProductSpecifications(ProductId,[Key],[Value])
VALUES
(2,N'RAM',N'8GB'),
(2,N'Bo nho',N'256GB');
```

### Kiem tra
- Detail co `images`, `specifications`, `relatedProducts`.
- `relatedProducts` la san pham cung category, khac id hien tai.

---

## 7) Bai 7 - Gio hang

### Endpoint (Customer)
- `POST /api/cart`
- `GET /api/cart`
- `PUT /api/cart?itemId={id}&quantity={qty}`
- `DELETE /api/cart?itemId={id}`

### Body them gio
```json
{
  "productId": 2,
  "quantity": 2
}
```

### Kiem tra
- Quantity <= 0 => loi.
- Quantity > stock => loi.
- `GET /api/cart` co tong tien chinh xac.

---

## 8) Bai 8 - Dat hang

### Endpoint
- `POST /api/orders` (Customer)
- `PUT /api/orders/{id}/status?status=Confirmed` (Admin,Staff)
- `GET /api/orders` (Customer - don cua minh)
- `GET /api/orders/all` (Admin,Staff)
- `GET /api/orders/{id}` (role-aware)

### Body tao don
```json
{
  "couponCode": null
}
```

### Kiem tra
- Dat tu cart.
- Het kho => bi chan.
- Dat thanh cong => tru kho + tao log inventory export.
- Status hop le: `Pending, Confirmed, Shipping, Completed, Cancelled`.

---

## 9) Bai 9 - Coupon

### Endpoint ap dung
- `POST /api/orders` voi body co `couponCode`

### Tao coupon (SQL)
```sql
INSERT INTO Coupons(Code,DiscountPercent,DiscountAmount,MinOrderValue,UsageLimit,UsedCount,ExpiryDate)
VALUES(N'SALE10',10,NULL,1000000,100,0,DATEADD(DAY,30,GETDATE()));
```

### Test
- Coupon ton tai + con han + du gia tri don + chua het luot => giam dung.
- Coupon sai code => `Coupon not found`.
- Het han => `Coupon expired`.
- Khong du min order => loi.

---

## 10) Bai 10 - Gia theo khu vuc

### Endpoint
- `GET /api/products?region=HCM`
- `GET /api/products/{id}/detail?region=HCM`

### Tao gia khu vuc (SQL)
```sql
INSERT INTO ProductRegionPrices(ProductId,Region,Price)
VALUES(2,N'HCM',24000000);
```

### Kiem tra
- Co gia khu vuc => finalPrice tinh tren gia khu vuc.
- Khong co gia khu vuc => fallback ve gia mac dinh.

---

## 11) Bai 11 - Tim cua hang

### Endpoint
- `GET /api/stores/by-city?city=HCM`
- `GET /api/stores/nearest?lat=10.7769&lng=106.7009`
- `GET /api/stores/has-product?productId=2`

### Seed store (SQL)
```sql
INSERT INTO Stores(Name,City,Latitude,Longitude)
VALUES
(N'Store Q1',N'HCM',10.7769,106.7009),
(N'Store Thu Duc',N'HCM',10.8500,106.7700);

INSERT INTO StoreInventories(StoreId,ProductId,Quantity)
VALUES(1,2,5),(2,2,0);
```

### Kiem tra
- by-city tra danh sach theo tinh/thanh.
- nearest tra 3 store gan nhat.
- has-product chi tra store con hang (`Quantity > 0`).

---

## 12) Bai 12 - Bao hanh

### Trang thai trong code hien tai
- Chua co `WarrantyController`/endpoint tra cuu bao hanh.

### Ket luan test
- Bai 12 hien `NOT IMPLEMENTED` trong source hien tai.
- Neu can nop du 16 bai dung nghia, ban can bo sung endpoint:
  - tra cuu theo serial/phone/orderCode
  - tra ve han bao hanh + trang thai con/het han.

---

## 13) Bai 13 - Mua tra gop

### Endpoint
- `POST /api/installments`

### Body
```json
{
  "productId": 2,
  "months": 12,
  "downPaymentPercent": 20,
  "customerName": "Nguyen Van A",
  "phone": "0909000000"
}
```

### Kiem tra
- Response co `price`, `downPayment`, `monthly`, `months`.
- DB co ban ghi `InstallmentRequests`.

---

## 14) Bai 14 - Dang nhap va phan quyen

### Role test
- Admin/Staff:
  - PASS: CRUD products, categories, promotions, inventory, reports.
- Customer:
  - PASS: cart, tao order, xem order cua minh.
  - FAIL ky vong (403): CRUD products/reports.

### Case bat buoc
1. Customer goi `POST /api/products` => 403.
2. Admin goi `POST /api/products` => 200.
3. Customer goi `GET /api/orders/all` => 403.
4. Customer goi `GET /api/orders/{id-cua-nguoi-khac}` => 403.

---

## 15) Bai 15 - Error handling va logging

### Case test
- 400: quantity <= 0 trong cart.
- 401: token sai/het han.
- 403: customer goi api admin.
- 404: `GET /api/products/999999/detail`.

### Test 500 (de demo)
- Tam them 1 endpoint test throw exception, goi 1 lan, chup log, xoa endpoint.
- Ky vong log co Error + path + traceId.

---

## 16) Bai 16 - Bao cao

### Endpoint (Admin,Staff)
- `GET /api/reports/revenue/daily?from=2026-04-01&to=2026-04-30`
- `GET /api/reports/revenue/monthly?from=2026-01-01&to=2026-12-31`
- `GET /api/reports/top-selling-products?top=5`
- `GET /api/reports/orders-by-status`
- `GET /api/reports/low-stock?threshold=5`

### Kiem tra
- Daily/monthly: tong doanh thu + so don dung theo khoang ngay.
- Top selling: sap xep giam dan theo so luong ban.
- Orders by status: gom theo trang thai.
- Low stock: san pham ton <= threshold.

---

## Checklist nop bai
- Source code day du.
- Migrations chay duoc tu dau (`dotnet ef database update`).
- File huong dan test: file nay.
- Anh chup: login role, tao product, cart->order, promotion finalPrice, reports.
- Neu giu dung yeu cau 16 bai: can bo sung bai 12 (warranty).
