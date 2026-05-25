# Frontend Deploy Checklist

## 1. Chuẩn bị

- Có URL backend thật, ví dụ `https://api.yourdomain.com`.
- Có URL frontend thật, ví dụ `https://app.yourdomain.com`.
- Backend đã bật CORS cho domain frontend.

## 2. Cấu hình env

- Tạo biến môi trường API base URL cho frontend.
- Ví dụ với Vite: `VITE_API_URL=https://api.yourdomain.com`.
- Không hardcode `localhost` trong source.

## 3. Gọi API

```ts
const API_BASE_URL = import.meta.env.VITE_API_URL;

fetch(`${API_BASE_URL}/api/auth/login`, {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({ username, password })
});
```

## 4. Build frontend

- Chạy lệnh build theo framework bạn dùng, ví dụ `npm run build` hoặc `pnpm build`.
- Kiểm tra biến môi trường production đã được nạp đúng trước khi build.
- Nếu deploy lên CDN hoặc static hosting, upload đúng thư mục build output.

## 5. Kiểm tra sau deploy

- Mở frontend và thử đăng nhập.
- Kiểm tra tab `Network` xem request có trỏ tới backend URL thật không.
- Kiểm tra console nếu có lỗi CORS.
- Kiểm tra token trả về có được lưu và gửi đúng trong header `Authorization`.

## 6. Lưu ý

- Nếu frontend đổi domain, cập nhật lại CORS ở backend.
- Không để source frontend phụ thuộc vào `localhost`.
- Nếu backend đổi domain, chỉ cần đổi `VITE_API_URL` rồi build lại frontend.
