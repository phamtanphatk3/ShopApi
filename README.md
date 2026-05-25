# ShopApi

ShopApi la du an ASP.NET Core Web API cho he thong ban hang thiet bi dien tu.

## Tong quan backend

- .NET `net10.0`
- ASP.NET Core Web API
- Entity Framework Core + SQL Server
- JWT Bearer Authentication
- FluentValidation
- Swagger / Swashbuckle
- CORS cho frontend goi API bang URL
- Repository + Service pattern

## Chuc nang backend da co

- Dang ky, dang nhap, lay profile, doi mat khau
- Danh muc, san pham, anh san pham
- Ton kho, gia theo khu vuc
- Gio hang, don hang, huy don
- Khuyen mai, ma giam gia
- Tra gop, bao hanh
- Cua hang, bao cao
- Danh sach yeu thich (`wishlist`)

## Thong tin login tra ve

`POST /api/auth/login` tra ve:

- `token`
- `user.id`
- `user.username`
- `user.role`
- `user.email`
- `user.phone`
- `user.address`

## User fields

Model `User` co:

- `Username`
- `Password`
- `Role`
- `Email`
- `Phone`
- `Address`

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

Neu can tao migration moi:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## CORS

Project da co CORS trong `Program.cs`. Origin cho phep nam trong `appsettings.json` va `appsettings.Production.json`.

## Luu y

- Controller chi xu ly HTTP concerns.
- Business logic nam trong `Services/`.
- Validation nam trong `Validators/`.
- Loi nghiep vu xu ly qua `ExceptionMiddleware`.
