using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Middlewares;
using ShopApi.Repositories;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services;
using ShopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;


namespace ShopApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Keep logging simple in dev/runtime to avoid Windows EventLog permission issues.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // ================= DB =================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            // ================= REPOSITORY =================
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // ================= SERVICE =================
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<ProductImageService>();
            builder.Services.AddScoped<InventoryService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<InstallmentService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<PromotionService>();
            builder.Services.AddScoped<WarrantyService>();

            builder.Services.AddHttpContextAccessor();

            // ================= CONTROLLER =================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "http://192.168.1.21:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Swagger + JWT Bearer support
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Dán trực tiếp: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference(
                            "Bearer",
                            document,
                            null!),
                        new List<string>()
                    }
                });
            });

            // ================= JWT =================
            var key = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key) || key.Length < 32)
                throw new Exception("JWT Key must be at least 32 characters");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // dev
                    options.SaveToken = true;
                    options.IncludeErrorDetails = true;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var authHeader = context.Request.Headers.Authorization.ToString();
                            if (string.IsNullOrWhiteSpace(authHeader))
                                return Task.CompletedTask;

                            // Swagger/UI thường thêm "Bearer " tự động.
                            // Hỗ trợ cả trường hợp người dùng lỡ nhập "Bearer <token>" vào ô Authorize.
                            if (authHeader.StartsWith("Bearer Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader["Bearer Bearer ".Length..].Trim();
                                return Task.CompletedTask;
                            }

                            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader["Bearer ".Length..].Trim();
                                return Task.CompletedTask;
                            }

                            // Fallback: nếu header không có prefix, dùng trực tiếp giá trị đó.
                            context.Token = authHeader.Trim();
                            return Task.CompletedTask;
                        }
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero, // 🔥 hết hạn là hết hạn luôn

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // ================= MIDDLEWARE =================

            app.UseMiddleware<ExceptionMiddleware>();

            // 👉 Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject.PersistAuthorization = true;
            });

            app.UseStaticFiles();
            app.UseCors("FrontendPolicy");

            // 🔥 QUAN TRỌNG: thứ tự này KHÔNG được sai
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
