# BAI 16 - Bao cao (Test Checklist)

## 1) Muc tieu
- Doanh thu theo ngay/thang
- Top san pham ban chay
- Don hang theo trang thai
- Danh sach ton kho thap

## 2) Dieu kien test
1. Chay API:
```powershell
dotnet run --project D:\ShopApi\ShopApi
```
2. Dang nhap bang tai khoan `Admin` hoac `Staff`.
3. Authorize JWT:
`Bearer <token>`
4. Database da co du lieu don hang + san pham.

## 3) Test case

### TC01 - Revenue by day
- API:
`GET /api/reports/revenue/daily?from=2026-04-01&to=2026-04-30`
- Ky vong:
  - HTTP `200`
  - `data[]` co: `date`, `revenue`, `orderCount`
- Actual: ...
- Result: PASS / FAIL

### TC02 - Revenue by month
- API:
`GET /api/reports/revenue/monthly?from=2026-04-01&to=2026-04-30`
- Ky vong:
  - HTTP `200`
  - `data[]` co: `year`, `month`, `revenue`, `orderCount`
- Actual: ...
- Result: PASS / FAIL

### TC03 - Top selling products
- API:
`GET /api/reports/top-selling-products?top=5`
- Ky vong:
  - HTTP `200`
  - `data[]` co: `productId`, `productName`, `sku`, `quantitySold`, `revenue`
  - Sap xep giam dan theo `quantitySold`
- Actual: ...
- Result: PASS / FAIL

### TC04 - Orders by status
- API:
`GET /api/reports/orders-by-status`
- Ky vong:
  - HTTP `200`
  - `data[]` co: `status`, `count`, `totalRevenue`
  - Tong `count` khop tong so don trong DB
- Actual: ...
- Result: PASS / FAIL

### TC05 - Low stock products
- API:
`GET /api/reports/low-stock?threshold=5`
- Ky vong:
  - HTTP `200`
  - Danh sach chi gom san pham co `stockQuantity <= 5`
- Actual: ...
- Result: PASS / FAIL

## 4) Kiem tra phan quyen

### TC06 - Khong token
- Goi `GET /api/reports/revenue/daily`
- Ky vong: `401`
- Result: PASS / FAIL

### TC07 - Token Customer
- Goi `GET /api/reports/revenue/daily`
- Ky vong: `403`
- Result: PASS / FAIL

### TC08 - Token Admin/Staff
- Goi `GET /api/reports/revenue/daily`
- Ky vong: `200`
- Result: PASS / FAIL

