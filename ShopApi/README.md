# ShopApi - Do an thuc hanh ASP.NET Core

## 1. Mo ta ngan
Du an API ban hang dien may, trien khai cac bai 1-16:
- CRUD danh muc, san pham, anh san pham
- Gio hang, dat hang, coupon
- Gia theo khu vuc, store, bao hanh/mo phong tra gop
- Dang nhap + phan quyen (Admin/Staff/Customer)
- Logging + global error handling
- Bao cao doanh thu, top ban chay, don theo trang thai, ton kho thap

## 2. Kien truc
Su dung mo hinh:
- `Controller -> Service -> Repository -> DbContext (EF Core)`

Thanh phan chinh:
- JWT Authentication + Role-based Authorization
- Global Exception Middleware
- API response format thong nhat
- EF Core Migrations

## 3. Yeu cau moi truong
- .NET SDK 10
- SQL Server
- EF Core Tools

Lenh cai EF Tool (neu chua co):
```bash
dotnet tool install --global dotnet-ef
```

## 4. Huong dan chay du an
1. Vao thu muc project:
```bash
cd D:\ShopApi\ShopApi
```

2. Restore package:
```bash
dotnet restore
```

3. Tao/cap nhat database:
```bash
dotnet ef database update
```

4. Chay API:
```bash
dotnet run
```

5. Mo Swagger:
- `https://localhost:7212/swagger`
- Neu profile may ban chay port khac, xem dong `Now listening on ...` trong console.

## 5. Tai khoan va phan quyen
Vai tro:
- `Admin`, `Staff`: CRUD san pham va cac API quan tri
- `Customer`: gio hang, dat hang, xem don cua chinh minh

Dang nhap:
- `POST /api/auth/login`
- Nhan token JWT, Authorize theo format:
`Bearer <token>`

## 6. Bao cao (Bai 16)
Endpoint:
- `GET /api/reports/revenue/daily`
- `GET /api/reports/revenue/monthly`
- `GET /api/reports/top-selling-products`
- `GET /api/reports/orders-by-status`
- `GET /api/reports/low-stock`

Quyen truy cap:
- `Admin`, `Staff`

## 7. Tai lieu test API
- Bai 15 checklist: `D:\ShopApi\BAI15_TEST.md`
- Bai 16 checklist: `D:\ShopApi\BAI16_TEST.md`

## 8. Ghi chu
- De reset du lieu cu:
```bash
dotnet ef database drop --force
dotnet ef database update
```
