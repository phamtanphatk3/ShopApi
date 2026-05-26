# Frontend Deploy Checklist - ShopApi

File nay chi danh cho frontend. Muc tieu la giup ban ket noi dung backend ShopApi qua URL that, khong phai `localhost`.

## 0. Dieu kien tien quyet

Truoc khi lam frontend, phai ro mot trong hai tinh huong sau:

- **Co backend da chay va co URL that**: frontend goi API truc tiep qua URL do.
- **Chua co backend**: frontend chi lam UI, mock data, hoac cho backend chay local truoc roi moi test API that.

Neu ban clone project ve may moi:

- Database khong nam trong repo.
- Phai co backend dang chay + database da tao xong thi frontend moi test du lieu that duoc.
- Neu chi co source frontend ma khong co backend/API URL, cac man login/san pham/gio hang se khong load duoc data that.

## 1. Ban can hieu gi ve backend nay

Backend ShopApi da co san cac nhom API chinh sau:

- Dang ky, dang nhap, lay profile, doi mat khau
- Danh sach san pham, chi tiet san pham, anh san pham, gia theo khu vuc
- Gio hang, don hang, huy don, yeu thich
- Ma giam gia, khuyen mai, cua hang
- Ton kho, tra gop, bao hanh, bao cao

Login backend hien tra ve:

```json
{
  "success": true,
  "message": "Dang nhap thanh cong",
  "data": {
    "token": "jwt-token",
    "user": {
      "id": 1,
      "username": "admin",
      "role": "Admin",
      "email": "admin@shopapi.com",
      "phone": "0900000000",
      "address": "Ho Chi Minh"
    }
  }
}
```

Lay thong tin tai khoan dang dang nhap qua:

- `GET /api/auth/me`

## 2. Du lieu mau co san trong backend

Neu frontend muon test nhanh, backend dang co data mau khi database chua co san pham.

Day la data that de frontend co the dung de test ngay, khong phai data demo tu phia frontend.

### Tai khoan mau

- `admin / 123`
- `staff / 123`
- `customer / 123`

### Danh muc mau

- `dien-thoai`
- `laptop`
- `phu-kien`

### San pham mau

- `iPhone 15 128GB`
- `Samsung Galaxy S24 256GB`
- `Laptop Dell Inspiron 14`
- `Sac nhanh 65W Type-C`

### Ma giam gia mau

- `WELCOME10`
- `SHIPFREE`

### Cua hang mau

- `ShopApi HCM - Q.1`
- `ShopApi HN - Hoan Kiem`

### Du lieu mau khac

- Gia theo khu vuc cho mot so san pham
- Anh san pham mau
- Ton kho mau
- Gio hang mau cua `customer`
- Don hang mau cua `customer`
- Bao hanh mau
- Tra gop mau

## 3. Bien moi truong frontend can co

Frontend chi can mot bien URL backend:

```bash
VITE_API_URL=https://api.shopapi.com
```

Neu dung framework khac, tao bien tuong duong theo quy uoc cua framework do.

Quy tac:

- Khong hardcode `localhost` trong source frontend
- Khong ghep chuoi URL thu cong o nhieu noi
- Tao 1 file client dung chung, vi du `api.ts` hoac `http.ts`
- Neu backend chua co URL that, tam thoi dung mock data cho giao dien

## 4. Cach frontend goi backend

### Login

```ts
const API_BASE_URL = import.meta.env.VITE_API_URL;

const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
  method: "POST",
  headers: {
    "Content-Type": "application/json"
  },
  body: JSON.stringify({
    username: "admin",
    password: "123"
  })
});

const result = await response.json();
const token = result.data.token;
const user = result.data.user;
```

### Goi API co token

```ts
const response = await fetch(`${API_BASE_URL}/api/auth/me`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});
```

Luu y:

- Neu frontend da tu them chuoi `Bearer ` thi backend se nhan dung format `Authorization: Bearer <token>`
- Khong gui token vao query string

## 5. Map theo tung man hinh frontend

### Man hinh auth

- Dang ky: `POST /api/auth/register`
- Dang nhap: `POST /api/auth/login`
- Xem profile: `GET /api/auth/me`
- Doi mat khau: `PUT /api/auth/me/password`

Du lieu login/register co the chua:

- `username`
- `password`
- `email`
- `phone`
- `address`

### Man hinh san pham

- Danh sach san pham: `GET /api/products`
- Chi tiet san pham: `GET /api/products/{id}/detail`
- Anh san pham: `GET /api/products/{productId}/images`
- Gia theo khu vuc: `GET /api/products/{productId}/region-prices`

### Man hinh danh muc

- Danh sach danh muc: `GET /api/categories`
- Chi tiet danh muc: `GET /api/categories/{id}`

### Man hinh gio hang

- Them vao gio: `POST /api/cart`
- Xem gio hang: `GET /api/cart`
- Cap nhat so luong: `PUT /api/cart`
- Xoa san pham khoi gio: `DELETE /api/cart`

### Man hinh don hang

- Tao don hang: `POST /api/orders`
- Xem don hang cua minh: `GET /api/orders`
- Xem chi tiet don hang: `GET /api/orders/{id}`
- Huy don: `PUT /api/orders/{id}/cancel`

### Man hinh yeu thich

- Xem danh sach yeu thich: `GET /api/wishlist`
- Them vao yeu thich: `POST /api/wishlist/{productId}`
- Xoa khoi yeu thich: `DELETE /api/wishlist/{productId}`

### Man hinh ma giam gia

- Validate ma: `GET /api/coupons/validate?code=WELCOME10`

### Man hinh cua hang

- Danh sach cua hang: `GET /api/stores`
- Chi tiet cua hang: `GET /api/stores/{id}`
- Tim cua hang theo tinh: `GET /api/stores/by-province?province=Ho%20Chi%20Minh`
- Tim cua hang gan nhat: `GET /api/stores/nearest?latitude=10.7758&longitude=106.7033`
- Kiem tra cua hang co san pham: `GET /api/stores/has-product?storeId=1&productId=2`

### Man hinh khuyen mai

- Danh sach khuyen mai: `GET /api/promotions`
- Chi tiet khuyen mai: `GET /api/promotions/{id}`

### Man hinh ton kho

- Nhap kho: `POST /api/products/{productId}/inventory/import`
- Xuat kho: `POST /api/products/{productId}/inventory/export`
- Lich su ton kho: `GET /api/products/{productId}/inventory/history`

### Man hinh tra gop

- Tao yeu cau tra gop: `POST /api/installments`

### Man hinh bao hanh

- Tao thong tin bao hanh: `POST /api/warranty`
- Tra cuu bao hanh: `GET /api/warranty/lookup?serialNumber=SN-SEED-000001`

### Man hinh bao cao

- Doanh thu theo ngay: `GET /api/reports/revenue/daily`
- Doanh thu theo thang: `GET /api/reports/revenue/monthly`
- San pham ban chay: `GET /api/reports/top-selling-products`
- Don hang theo trang thai: `GET /api/reports/orders-by-status`
- San pham sap het hang: `GET /api/reports/low-stock`

## 6. Chu an request frontend nen dung

Tam thoi frontend nen co 1 helper chung:

```ts
const API_BASE_URL = import.meta.env.VITE_API_URL;

async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, init);

  const data = await response.json();

  if (!response.ok) {
    throw data;
  }

  return data;
}
```

Mau header can dung:

```ts
{
  "Content-Type": "application/json",
  "Authorization": `Bearer ${token}`
}
```

## 7. Build frontend

Chay lenh build theo framework:

- Vite: `npm run build`
- React: `npm run build`
- Vue: `npm run build`
- Next.js: `npm run build`

Checklist truoc khi build:

- Da set dung `VITE_API_URL`
- Da dang nhap tren backend that
- Da test it nhat 3 man hinh: login, san pham, gio hang

## 8. Kiem tra sau khi deploy frontend

Kiem tra tren trinh duyet:

- Login thanh cong va lay duoc `token` + `user`
- Trang san pham load duoc data that
- Trang chi tiet san pham hien anh + gia + thong so
- Them vao gio hang khong bi loi CORS
- Tao don hang nhan duoc response hop le
- Trang profile lay duoc `email`, `phone`, `address`
- Neu mot so man co backend chua hoan thien, frontend can co trang thai `loading`, `empty` va `error` ro rang thay vi treo trang

Kiem tra trong DevTools:

- Request dang goi dung `VITE_API_URL`
- Header co `Authorization`
- Response co du lieu trong `data`
- Khong con hardcode `localhost`

## 9. Loi thuong gap o frontend

- `CORS error`: domain frontend chua duoc backend allow
- `401 Unauthorized`: chua gui token hoac token het han
- `403 Forbidden`: tai khoan khong co quyen
- `400 Bad Request`: payload gui sai truong hoac sai format
- `404 Not Found`: sai route
- `500 Internal Server Error`: backend bi loi, can xem `message` va `traceId`

## 10. Thu tu lam viec de frontend de hieu

1. Tao file env cua frontend: `VITE_API_URL`
2. Tao API client chung
3. Lam man hinh login
4. Luu `token` va `user`
5. Lam man hinh san pham
6. Lam man hinh chi tiet san pham
7. Lam gio hang
8. Lam don hang
9. Lam profile
10. Lam yeu thich, coupon, cua hang neu can

## 11. Neu backend chua co thi lam gi truoc

- Lam UI theo mock data truoc de khong bi chan tien do.
- Chuan bi san file env de sau nay chi can doi `VITE_API_URL`.
- Luu san cac man hinh can API nao de khi backend co URL la gan vao ngay.
- Khong can co database de frontend lam giao dien, nhung se can database neu muon test luong that.
