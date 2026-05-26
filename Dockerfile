# Giai đoạn 1: Chạy ứng dụng (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Giai đoạn 2: Build code ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file project vào trước để khôi phục dependencies (tối ưu cache Docker)
COPY ["ShopApi.csproj", "."]
RUN dotnet restore "ShopApi.csproj"

# Copy toàn bộ code còn lại vào và tiến hành build
COPY . .
WORKDIR "/src/."
RUN dotnet build "ShopApi.csproj" -c Release -o /app/build

# Giai đoạn 3: Xuất bản file chạy (.dll sạch)
FROM build AS publish
RUN dotnet publish "ShopApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn cuối: Copy file chạy vào môi trường Runtime tinh gọn
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopApi.dll"]