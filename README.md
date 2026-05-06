# ShopApi - Backend bai tap 1-16

## 1) Mo ta ngan
ShopApi la backend ASP.NET Core + EF Core cho bai tap Shop dien may (bai 1 den bai 16), gom:
- CRUD Category/Product/ProductImage
- Cart/Order/Coupon
- Promotion + gia sau giam
- Gia theo khu vuc
- Store + ton kho theo cua hang
- Warranty lookup
- Installment simulation
- Auth + phan quyen Admin/Staff/Customer
- Logging + global exception middleware
- Reports (doanh thu, top ban chay, don theo trang thai, ton kho thap)

## 2) Kien truc ngan gon
- `Controllers/`: nhan request, authorize, tra response.
- `Services/`: xu ly nghiep vu.
- `Repositories/`: truy cap du lieu cho mot so module.
- `Data/AppDbContext`: cau hinh EF Core, relation, index, seed.
- `Models/`: entity map DB.
- `DTOs/`: model request/response cho API.
- `Middlewares/ExceptionMiddleware`: bat loi toan cuc, tra format loi thong nhat.
- `Validators/` + FluentValidation: validate input.

Flow: `Client -> Controller -> Service -> DbContext/Repository -> SQL Server -> Response`.

## 3) Yeu cau moi truong
- .NET SDK 10
- SQL Server
- dotnet-ef

Lenh cai dotnet-ef (neu chua co):
```bash
dotnet tool install --global dotnet-ef
```

## 4) Cach chay du an
1. Mo terminal:
```bash
cd D:\ShopApi\ShopApi
```

2. Restore:
```bash
dotnet restore
```

3. Migrate DB:
```bash
dotnet ef database update
```

4. Run API:
```bash
dotnet run
```

5. Mo Swagger:
- Localhost: `https://localhost:7212/swagger/index.html`
- LAN (theo launch profile hien tai): `https://192.168.1.32:7212/swagger/index.html`

Neu port/IP khac, xem log `Now listening on ...` trong console.

## 5) Migration va reset DB
Reset sach DB:
```bash
dotnet ef database drop --force
dotnet ef database update
```

Danh sach migration trong thu muc:
- `D:\ShopApi\ShopApi\Migrations`

## 6) Seed du lieu
Sau `dotnet ef database update`, du lieu seed co san tu migration/model config:
- Category mau
- User mau (neu chua ton tai) duoc chen bo sung trong migration `AddUserIdToCart`

Neu can tao them user Staff bang SQL:
```sql
INSERT INTO Users(Username, Password, Role)
VALUES ('staff01', '123', 'Staff');
```

## 7) Tai khoan mau
Mac dinh dung de test:
- Admin: `admin / 123`
- Customer: `user1 / 123`

Dang nhap:
- `POST /api/auth/login`

Body:
```json
{
  "username": "admin",
  "password": "123"
}
```

Lay token va truyen:
```text
Authorization: Bearer <token>
```

## 8) Link test API nhanh
- Swagger UI:
  - `https://localhost:7212/swagger/index.html`
  - `https://192.168.1.32:7212/swagger/index.html`
- Auth:
  - `POST /api/auth/login`
- Products listing:
  - `GET /api/products`
- Product detail:
  - `GET /api/products/{id}/detail`
- Cart:
  - `POST /api/cart`
  - `GET /api/cart`
- Orders:
  - `POST /api/orders`
  - `GET /api/orders`
- Reports:
  - `GET /api/reports/revenue/daily`
  - `GET /api/reports/revenue/monthly`
  - `GET /api/reports/top-selling-products`
  - `GET /api/reports/orders-by-status`
  - `GET /api/reports/low-stock`

## 9) Tai lieu test bo sung
- Checklist bai 15: `D:\ShopApi\BAI15_TEST.md`

