# ShopApi

ShopApi là ASP.NET Core Web API cho hệ thống bán hàng thiết bị điện tử.

## Quy trình

- `ShopApi` quản lý sản phẩm, giỏ hàng, đơn hàng, coupon, khuyến mãi, tồn kho, bảo hành, trả góp, cửa hàng và báo cáo.
- Backend dùng `.NET 10`, `Entity Framework Core`, `SQL Server`, `JWT`, `FluentValidation`, `Swagger`, `CORS`.
- Login trả về `token` và `user` gồm `id`, `username`, `role`, `email`, `phone`, `address`.
- Mặc định API bị khóa bằng JWT; endpoint public phải gắn `[AllowAnonymous]`.
- `Program.cs` đã cấu hình CORS, Swagger, auth, validation, DI.

## Chức năng chính

- Auth: đăng ký, đăng nhập, profile, đổi mật khẩu
- Products: danh sách, chi tiết, ảnh, giá theo khu vực
- Cart / Orders / Wishlist
- Coupons / Promotions / Stores
- Inventory / Warranty / Installments / Reports
- Categories / Product region prices / Product images

## Bảng quyền truy cập API ngắn

| Nhóm | Endpoint ví dụ | Cần token | Quyền |
|---|---|---:|---|
| Public | `POST /api/auth/login` | Không | - |
| Public | `GET /api/coupons/validate` | Không / tùy flow | - |
| User | `GET /api/auth/me` | Có | User |
| User | `POST /api/cart` | Có | User |
| User | `POST /api/orders` | Có | User |
| User | `GET /api/wishlist` | Có | User |
| Admin/Staff | `POST /api/products` | Có | Admin/Staff |
| Admin/Staff | `DELETE /api/coupons/{id}` | Có | Admin/Staff |
| Admin/Staff | `POST /api/stores` | Có | Admin/Staff |
| Admin/Staff | `GET /api/reports/revenue/daily` | Có | Admin/Staff |

## Chạy local

```powershell
dotnet restore
dotnet build
dotnet run
```

## Database

```powershell
dotnet ef database update
```

Tạo migration mới:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Dữ liệu mẫu Development

Khi chạy ở Development, app có thể tự seed dữ liệu mẫu nếu bảng sản phẩm còn trống:

- `admin / 123`
- `staff / 123`
- `customer / 123`
- `WELCOME10`
- `SHIPFREE`

## CORS

Origin cho phép nằm trong `appsettings.json` và `appsettings.Production.json`.

## Lưu ý kỹ thuật

- Controller chỉ xử lý HTTP.
- Business logic nằm trong `Services/`.
- Validation nằm trong `Validators/`.
- Lỗi nghiệp vụ đi qua `ExceptionMiddleware`.
