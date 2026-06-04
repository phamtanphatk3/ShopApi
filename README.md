# ShopApi

ShopApi la ASP.NET Core Web API cho he thong ban hang thiet bi dien tu.

## 1 phut la hieu

- `ShopApi` quan ly san pham, gio hang, don hang, coupon, khuyen mai, ton kho, bao hanh, tra gop, cua hang va bao cao.
- Backend dung `.NET 10`, `Entity Framework Core`, `SQL Server`, `JWT`, `FluentValidation`, `Swagger`, `CORS`.
- Login tra ve `token`, `refreshToken` va `user` gom `id`, `username`, `role`, `email`, `phone`, `address`.
- Mac dinh API bi khoa bang JWT; endpoint public phai gan `[AllowAnonymous]`.
- `Program.cs` da cau hinh CORS, Swagger, auth, validation, DI.

## Chuc nang chinh

- Auth: dang ky, dang nhap, profile, doi mat khau, refresh token, logout
- Products: danh sach, chi tiet, anh, gia theo khu vuc
- Cart / Orders / Wishlist
- Coupons / Promotions / Stores
- Inventory / Warranty / Installments / Reports
- Categories / Product region prices / Product images / Order history / Health check

## Bang quyen truy cap API ngan

| Nhom | Endpoint vi du | Can token | Quyen |
|---|---|---:|---|
| Public | `POST /api/auth/login` | Khong | - |
| Public | `GET /api/health` | Khong | - |
| User | `GET /api/auth/me` | Co | User |
| User | `POST /api/auth/refresh` | Co refresh token | User |
| User | `POST /api/cart` | Co | User |
| User | `POST /api/orders` | Co | User |
| User | `GET /api/wishlist` | Co | User |
| Admin/Staff | `POST /api/products` | Co | Admin/Staff |
| Admin/Staff | `DELETE /api/coupons/{id}` | Co | Admin/Staff |
| Admin/Staff | `POST /api/stores` | Co | Admin/Staff |
| Admin/Staff | `GET /api/reports/revenue/daily` | Co | Admin/Staff |

## Chay local

```powershell
dotnet restore
dotnet build
dotnet run
```

## Database

```powershell
dotnet ef database update
```

Tao migration moi:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Du lieu mau Development

Khi chay o Development, app co the seed du lieu mau neu bang san pham con trong:

- `admin / 123`
- `staff / 123`
- `customer / 123`
- `WELCOME10`
- `SHIPFREE`

## CORS

Origin cho phep nam trong `appsettings.json` va `appsettings.Production.json`.

## Luu y ky thuat

- Controller chi xu ly HTTP.
- Business logic nam trong `Services/`.
- Validation nam trong `Validators/`.
- Loi nghiep vu di qua `ExceptionMiddleware`.
