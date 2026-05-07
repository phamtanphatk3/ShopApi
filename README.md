# ShopApi Backend (ASP.NET Core 10)

## 1. Tong quan
ShopApi la backend cho bai tap thuc hanh shop dien may (bai 1-16), su dung:
- ASP.NET Core 10 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication + Role-based Authorization

## 2. Cau truc du an
Thu muc chinh trong `D:\ShopApi\ShopApi`:
- `Controllers`: endpoint API
- `Services`: xu ly nghiep vu
- `Repositories`: truy cap du lieu (Category/Product)
- `Data`: `AppDbContext`, cau hinh EF Core
- `Models`: entity map bang SQL
- `DTOs`: request/response model cho API
- `Validators`: FluentValidation
- `Middlewares`: global exception middleware
- `Common`: ApiResponse, ApiErrorResponse, helper
- `Migrations`: lich su thay doi schema DB
- `wwwroot`: file tinh (anh san pham)

Flow: `Controller -> Service -> Repository/DbContext -> SQL Server`.

## 3. Yeu cau moi truong
- .NET SDK 10
- SQL Server
- dotnet-ef

Cai dat `dotnet-ef` (neu chua co):
```bash
dotnet tool install --global dotnet-ef
```

## 4. Cau hinh
File chinh:
- `D:\ShopApi\ShopApi\appsettings.json`
- `D:\ShopApi\ShopApi\appsettings.Development.json`

Can kiem tra:
- `ConnectionStrings:Default`
- `Jwt:Key` (do dai >= 32 ky tu)

## 5. Chay du an
```bash
cd D:\ShopApi\ShopApi
dotnet restore
dotnet ef database update
dotnet run
```

Swagger:
- `https://localhost:7212/swagger/index.html`

## 6. Reset database
```bash
cd D:\ShopApi\ShopApi
dotnet ef database drop --force
dotnet ef database update
```

## 7. Tai khoan
- Admin: `admin / 123`
- Customer: `user1 / 123`

Dang nhap:
- `POST /api/auth/login`

Body mau:
```json
{
  "username": "admin",
  "password": "123"
}
```

Header sau khi login:
```text
Authorization: Bearer <token>
```

## 8. Phan quyen
- Mac dinh endpoint yeu cau dang nhap JWT
- `[AllowAnonymous]`: chi `login` va `register`
- `Admin/Staff`: API quan tri (CRUD san pham, promotion, report, inventory...)
- `Customer`: gio hang, dat hang, xem don cua minh

## 9. API chinh
- Auth:
  - `POST /api/auth/register`
  - `POST /api/auth/login`
- Categories:
  - `GET /api/categories`
  - `GET /api/categories/{id}`
  - `POST /api/categories`
  - `PUT /api/categories/{id}`
  - `DELETE /api/categories/{id}`
- Products:
  - `GET /api/products`
  - `GET /api/products/{id}/detail`
  - `POST /api/products`
  - `PUT /api/products/{id}`
  - `DELETE /api/products/{id}`
- Product Images:
  - `POST /api/products/{productId}/images`
  - `GET /api/products/{productId}/images`
- Promotions:
  - `POST /api/promotions`
  - `POST /api/promotions/{promoId}/products/{productId}`
- Cart:
  - `POST /api/cart`
  - `GET /api/cart`
  - `PUT /api/cart`
  - `DELETE /api/cart`
- Orders:
  - `POST /api/orders`
  - `GET /api/orders`
  - `GET /api/orders/{id}`
  - `GET /api/orders/all`
  - `PUT /api/orders/{id}/status`
- Coupons:
  - `GET /api/coupons/validate?code=...&orderAmount=...`
- Region Prices:
  - `GET /api/products/{productId}/region-prices`
  - `POST /api/products/{productId}/region-prices`
  - `PUT /api/products/{productId}/region-prices/{region}`
  - `DELETE /api/products/{productId}/region-prices/{region}`
- Stores:
  - `GET /api/stores/by-province`
  - `GET /api/stores/nearest`
  - `GET /api/stores/has-product`
- Warranty:
  - `POST /api/warranty`
  - `GET /api/warranty/lookup`
- Reports:
  - `GET /api/reports/revenue/daily`
  - `GET /api/reports/revenue/monthly`
  - `GET /api/reports/top-selling-products`
  - `GET /api/reports/orders-by-status`
  - `GET /api/reports/low-stock`

## 10. Tinh nang da co theo bai 1-16
- CRUD danh muc + rang buoc xoa mem
- CRUD san pham + search/filter/sort/paging
- Anh san pham (upload, isMain, sortOrder)
- Khuyen mai va gia sau giam
- Chi tiet san pham + specs + related
- Gio hang va dat hang tu gio
- Coupon validation
- Gia theo khu vuc + fallback
- Store lookup + ton kho
- Warranty lookup
- Installment simulation
- JWT auth + role
- Global exception + log
- Bao cao + memory cache

## 11. Loi thuong gap
- `401`: thieu token, token sai/het han, sai dinh dang Bearer
- `403`: token dung nhung sai role
