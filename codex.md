# Codex Project Guide - ShopApi

## Tổng quan

ShopApi là dự án ASP.NET Core Web API cho hệ thống bán hàng thiết bị điện tử. API quản lý danh mục, sản phẩm, ảnh sản phẩm, tồn kho, giỏ hàng, đơn hàng, khuyến mãi, mã giảm giá, trả góp, bảo hành, cửa hàng, giá theo khu vực, báo cáo, danh sách yêu thích và xác thực người dùng.

Công nghệ chính:

- .NET `net10.0`
- ASP.NET Core Web API
- Entity Framework Core với SQL Server
- JWT Bearer Authentication
- FluentValidation
- Swagger / Swashbuckle
- CORS cho frontend gọi API qua URL thật
- Repository + Service pattern

## Sơ đồ cây thư mục

```text
ShopApi/
|-- codex.md                         <- bộ não và quy tắc làm việc cho Codex trong dự án này
|-- README.md                        <- tong quan backend va huong dan chay
|-- SHOPAPI_FULL_TEST.md             <- checklist test backend day du
|-- Program.cs                       <- điểm khởi động, DI, middleware, auth, swagger, cors
|-- ShopApi.csproj                   <- cấu hình target framework và NuGet packages
|-- appsettings.json                 <- cấu hình chung: connection string, JWT, CORS
|-- appsettings.Development.json     <- cấu hình môi trường development
|-- appsettings.Production.json      <- cấu hình môi trường production
|-- ShopApi.http                     <- file test request nhanh trong IDE
|-- swagger-debug.json               <- swagger export/debug
|
|-- Common/                          <- lớp dùng chung
|   |-- ApiResponse.cs               <- response wrapper thành công
|   |-- ApiErrorResponse.cs          <- response wrapper lỗi
|   |-- PasswordHelper.cs            <- tiện ích hash/verify mật khẩu
|   |-- Auth/
|   |   `-- DevAuthHandler.cs        <- auth handler phục vụ development
|   `-- Exceptions/
|       `-- AppExceptions.cs         <- exception nghiệp vụ custom
|
|-- Controllers/                     <- HTTP endpoints
|   |-- AuthController.cs
|   |-- ProductsController.cs
|   |-- CategoriesController.cs
|   |-- CartController.cs
|   |-- OrdersController.cs
|   |-- InventoryController.cs
|   |-- CouponsController.cs
|   |-- PromotionsController.cs
|   |-- ReportsController.cs
|   |-- StoresController.cs
|   |-- ProductImagesController.cs
|   |-- ProductRegionPricesController.cs
|   |-- WishlistController.cs
|   |-- InstallmentsController.cs
|   |-- WarrantyController.cs
|   `-- ...
|
|-- Data/                            <- EF Core DbContext và factory
|   |-- AppDbContext.cs              <- DbSet, mapping quan hệ, index, seed data
|   |-- AppDbContextFactory.cs       <- factory phục vụ migrations/design time
|   `-- DbSeeder.cs                  <- seed du lieu mau cho Development
|
|-- DTOs/                            <- request/response contract theo từng domain
|   |-- Auth/
|   |-- Coupon/
|   |-- Product/
|   |-- Category/
|   |-- Cart/
|   |-- Order/
|   |-- Inventory/
|   |-- Installment/
|   |-- ProductImage/
|   |-- Report/
|   |-- Store/
|   |-- Wishlist/
|   `-- Warranty/
|
|-- Middlewares/
|   `-- ExceptionMiddleware.cs       <- bắt exception tập trung và trả ApiResponse lỗi
|
|-- Models/                          <- entity EF Core
|   |-- Product.cs
|   |-- Category.cs
|   |-- User.cs
|   |-- Cart.cs
|   |-- Order.cs
|   |-- Promotion.cs
|   |-- Coupon.cs
|   |-- Store.cs
|   |-- WishlistItem.cs
|   |-- WarrantyRecord.cs
|   `-- ...
|
|-- Repositories/                    <- tầng truy cập dữ liệu
|   |-- Interfaces/
|   |   |-- IProductRepository.cs
|   |   `-- ICategoryRepository.cs
|   |-- ProductRepository.cs
|   `-- CategoryRepository.cs
|
|-- Services/                        <- nghiệp vụ ứng dụng
|   |-- Interfaces/
|   |   |-- IProductService.cs
|   |   `-- ICategoryService.cs
|   |-- ProductService.cs
|   |-- CategoryService.cs
|   |-- AuthService.cs
|   |-- CartService.cs
|   |-- CouponService.cs
|   |-- OrderService.cs
|   |-- InventoryService.cs
|   |-- PromotionService.cs
|   |-- StoreService.cs
|   |-- WishlistService.cs
|   |-- ReportService.cs
|   |-- ProductImageService.cs
|   |-- ProductRegionPriceService.cs
|   |-- InstallmentService.cs
|   |-- WarrantyService.cs
|   `-- ...
|
|-- Validators/                      <- FluentValidation validators
|   |-- AuthValidators.cs
|   |-- ProductValidators.cs
|   |-- CategoryValidators.cs
|   |-- CartValidators.cs
|   |-- CommerceValidators.cs
|   |-- MediaValidators.cs
|   |-- SalesFlowValidators.cs
|   |-- WarrantyValidators.cs
|   `-- ...
|
|-- Migrations/                      <- EF Core migrations
|
|-- wwwroot/
|   `-- images/                      <- static files ảnh sản phẩm/thương hiệu
|
|-- bin/                             <- output build, không sửa tay
`-- obj/                             <- file tạm build/restore, không sửa tay
```

## Quy tắc làm việc cho Codex

- Luôn đọc code hiện có trước khi sửa, ưu tiên pattern đang có trong `Controllers`, `Services`, `Repositories`, `DTOs`, `Validators`.
- Không sửa file trong `bin/`, `obj/`, `.vs/` trừ khi người dùng yêu cầu rõ.
- Không đưa secret thật vào repo. JWT key, connection string và CORS origin production cần nằm trong configuration theo môi trường.
- Khi thêm endpoint mới, tạo đầy đủ các phần liên quan: `Controller`, `DTO`, `Service`, `Validator`, `Model/DbContext/Migration` nếu có thay đổi database.
- Controller chỉ xử lý HTTP concerns: route, auth attribute, status code, gọi service và bọc `ApiResponse`.
- Nghiệp vụ, tính toán giá, tồn kho, khuyến mãi, đơn hàng, quyền truy cập phải nằm trong service.
- Repository chỉ nên phụ trách truy vấn/persist entity. Không đưa validation nghiệp vụ vào repository.
- Lỗi nghiệp vụ nên dùng exception custom trong `Common/Exceptions`, để `ExceptionMiddleware` xử lý tập trung.
- Response thành công nên thống nhất theo `ApiResponse<T>` với `Success`, `Message`, `Data`.
- Validation request nên viết bằng FluentValidation trong `Validators/`, tránh rải validation lặp lại trong controller.
- Mặc định endpoint yêu cầu JWT do `FallbackPolicy`; endpoint public phải gắn `[AllowAnonymous]`.
- Endpoint quản trị/staff dùng `[Authorize(Roles = "Admin,Staff")]` hoặc role phù hợp.
- Khi dùng EF Core, ưu tiên async API: `ToListAsync`, `FirstOrDefaultAsync`, `AnyAsync`, `SaveChangesAsync`.
- Khi lấy dữ liệu liên quan, dùng `Include`/`ThenInclude` có chủ đích và tránh load thừa.
- Khi thay đổi schema database, cập nhật `AppDbContext`, model và tạo migration tương ứng.

## Quy ước code

- Namespace gốc: `ShopApi`.
- Bật `Nullable` và `ImplicitUsings` theo `.csproj`; code mới cần tôn trọng nullable reference types.
- Đặt tên class theo domain: `ProductService`, `ProductCreateDto`, `ProductResponseDto`, `ProductValidator`.
- Method bất đồng bộ dùng hậu tố `Async`.
- DTO request không nên expose trực tiếp entity EF Core.
- Chuỗi thông báo API hiện đang dùng tiếng Việt có dấu ở một số nơi và không dấu ở nhiều nơi; khi sửa gần code cũ, giữ style thông báo nhất quán với file đang sửa.
- Comment ngắn gọn chỉ dùng khi cần giải thích ý nghĩa nghiệp vụ hoặc đoạn logic khó đọc.

## Luồng request chuẩn

```text
Client
  -> Controller
  -> FluentValidation
  -> Service
  -> Repository / AppDbContext
  -> SQL Server
  -> Service map sang DTO
  -> Controller trả ApiResponse
```

## Khi thêm một tính năng mới

1. Xác định domain và route trong `Controllers/`.
2. Tạo hoặc cập nhật DTO trong `DTOs/<Domain>/`.
3. Thêm validator trong `Validators/`.
4. Thêm method service trong `Services/` và interface nếu domain đang có interface.
5. Thêm repository/interface nếu cần tái sử dụng truy vấn phức tạp.
6. Cập nhật `Program.cs` để đăng ký DI nếu thêm service/repository mới.
7. Nếu thêm entity/bảng/cột, cập nhật `Models/`, `Data/AppDbContext.cs` và tạo migration.
8. Kiểm tra Swagger và chạy build.

## Lệnh thường dùng

```powershell
dotnet restore
dotnet build
dotnet run
```

Chạy migration EF Core:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Nếu `dotnet ef` chưa có:

```powershell
dotnet tool install --global dotnet-ef
```

## Điểm cần chú ý trong dự án này

- `Program.cs` đang cấu hình JWT bắt buộc và yêu cầu `Jwt:Key` dài ít nhất 32 ký tự.
- `Program.cs` đã bật CORS bằng `Cors:AllowedOrigins`.
- Swagger lưu authorization token bằng `PersistAuthorization = true`.
- Static file ảnh được phục vụ từ `wwwroot/images`.
- `ProductService` có logic giá theo khu vực và khuyến mãi; khi sửa giá sản phẩm cần xem kỹ logic `GetPriceByRegion` và `CalculatePriceWithBase`.
- `AppDbContext` có unique index cho `Category.Slug`, `WarrantyRecord.SerialNumber`, và `WishlistItem(UserId, ProductId)`.
- `User` đã có thêm `Email`, `Phone`, `Address`.
- Login trả về `token` kèm `user` đầy đủ (`id`, `username`, `role`, `email`, `phone`, `address`).
- `GET /api/auth/me` và `PUT /api/auth/me/password` đã có sẵn.
- `WishlistItems` là bảng mới cho danh sách yêu thích.
- `Orders` có thêm hủy đơn và hoàn kho.
- `Coupons`, `Promotions`, `Stores`, `ProductImages` đã có thêm endpoint CRUD/quản trị cần thiết.
- `DbSeeder` tự seed dữ liệu mẫu trong Development khi bảng sản phẩm còn trống.
- Một số service đang truy cập trực tiếp `AppDbContext`, một số qua repository. Khi mở rộng domain, ưu tiên giữ đúng pattern của domain đang sửa.

## Việc Codex nên tránh

- Không tạo framework mới, layer mới hoặc pattern mới nếu pattern hiện tại đã đáp ứng.
- Không đổi route/API contract cũ nếu không được yêu cầu, trừ khi đang mở rộng feature backend đã thiếu theo yêu cầu người dùng.
- Không sửa migration cũ đã tồn tại để "làm đẹp"; nếu schema thay đổi, tạo migration mới.
- Không format lại hàng loạt file ngoài phạm vi yêu cầu.
- Không thêm package mới khi có thể giải quyết bằng package sẵn có.
