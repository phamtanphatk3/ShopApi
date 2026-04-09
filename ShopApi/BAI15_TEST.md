# Logging và xử lý lỗi

## Run project
- dotnet ef database update
- dotnet run

## API
- GET /api/categories# BAI 15 - Logging va Xu ly loi (Test Checklist)

## 1) Muc tieu bai 15
- Global exception middleware hoat dong.
- Response loi theo format thong nhat.
- Log duoc loi quan trong (dac biet loi 500).

## 2) Chuan bi
1. Chay API:
```powershell
dotnet run --project D:\ShopApi\ShopApi
```
2. Kiem tra port app dang listen trong console.
- Vi du profile hien tai cua ban thuong la: `http://localhost:5132`
- Neu may ban dang chay SSL profile thi co the la: `https://localhost:7212`

## 3) Mau response loi chuan 
```json
{
  "success": false,
  "message": "Noi dung loi",
  "traceId": "...",
  "path": "...",
  "timestamp": "..."
}
```

## 4) Test case chi tiet

### TC01 - 401 Unauthorized (khong token)
- Endpoint: `POST /api/products`
- Dieu kien: Khong gui token.
- Body:
```json
{
  "name": "Test 401",
  "sku": "TC01-401",
  "brand": "Samsung",
  "price": 1000000,
  "stockQuantity": 1,
  "description": "test",
  "categoryId": 1
}
```
- Ky vong:
  - HTTP `401`
  - Header co `www-authenticate: Bearer`

### TC02 - 400 BadRequest (loi nghiep vu, SKU trung)
- Endpoint: `POST /api/products`
- Dieu kien: Dang nhap Admin/Staff va gui token dung format:
`Authorization: Bearer <token>`
- Body (SKU da ton tai):
```json
{
  "name": "Test Duplicate SKU",
  "sku": "SS-S24",
  "brand": "Samsung",
  "price": 10000000,
  "stockQuantity": 1,
  "description": "test 400 duplicate sku",
  "categoryId": 1
}
```
- Ky vong:
  - HTTP `400`
  - `message` thong bao SKU da ton tai (hoac tuong duong)

### TC03 - 404 NotFound (khong tim thay du lieu)
- Endpoint: `GET /api/products/999999/detail`
- Ky vong:
  - HTTP `404` (neu mapping NotFound dung)
  - Noi dung thong bao khong tim thay san pham

### TC04 - 500 InternalServerError (loi he thong bat ngo)
- Cach test:
  - Tam thoi them endpoint test 500 (hoac tao action throw exception noi bo).
  - Goi endpoint test do.
- Ky vong:
  - HTTP `500`
  - Body theo format loi chuan (`success=false`, `traceId`, `path`, `timestamp`)

### TC05 - Logging loi quan trong
- Thao tac: Goi TC04 de tao loi 500.
- Ky vong tren console:
  - Co log muc `Error`
  - Co `path` endpoint
  - Co `traceId` trung voi response


## 5) Ghi chu quan trong
- Khi test endpoint can quyen, token phai dung format:
`Authorization: Bearer <token>`
- Neu gap 401 + `invalid_token`:
  - Dang nhap lai lay token moi
  - Dam bao app dang chay dung host/port
  - Dam bao key JWT tao token va verify token la cung mot key
- POST /api/categories
- PUT /api/categories/{id}
- DELETE /api/categories/{id}

## Tech
- ASP.NET Core
- EF Core
- SQL Server