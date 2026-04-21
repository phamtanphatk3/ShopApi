using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ShopApi.Common;
using ShopApi.Common.Auth;
using ShopApi.Data;
using ShopApi.Middlewares;
using ShopApi.Repositories;
using ShopApi.Repositories.Interfaces;
using ShopApi.Services;
using ShopApi.Services.Interfaces;
using ShopApi.Validators;
using System.Text;


namespace ShopApi
{
    // Diem khoi dong chinh cua ung dung ASP.NET Core.
    public class Program
    {
        // Cau hinh DI, middleware, auth, swagger va chay web app.
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cau hinh logging don gian de tranh loi quyen EventLog tren Windows.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Dang ky DbContext ket noi SQL Server.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            // Dang ky repository.
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // Dang ky service nghiep vu.
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
            builder.Services.AddScoped<StoreService>();
            builder.Services.AddScoped<ProductRegionPriceService>();

            builder.Services.AddHttpContextAccessor();

            // Dang ky controller va FluentValidation.
            builder.Services.AddControllers();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .Select(x => new
                        {
                            Field = x.Key,
                            Errors = x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                        })
                        .ToList();

                    return new BadRequestObjectResult(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Du lieu dau vao khong hop le",
                        Data = errors
                    });
                };
            });
            builder.Services.AddFluentValidationAutoValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
            });
            builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://192.168.1.21:3000",
                            "http://192.168.1.26:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Cau hinh Swagger va co che Authorize token.
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

            // Cau hinh auth: DevAuth khi development (neu bat), nguoc lai dung JWT.
            var bypassAuthInDevelopment =
                builder.Environment.IsDevelopment() &&
                builder.Configuration.GetValue<bool>("Auth:BypassEnabled");

            if (bypassAuthInDevelopment)
            {
                builder.Services
                    .AddAuthentication("DevAuth")
                    .AddScheme<AuthenticationSchemeOptions, DevAuthHandler>("DevAuth", _ => { });
            }
            else
            {
                var key = builder.Configuration["Jwt:Key"];

                if (string.IsNullOrEmpty(key) || key.Length < 32)
                    throw new Exception("JWT Key phai dai it nhat 32 ky tu");

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false; // Chi dung cho moi truong development.
                        options.SaveToken = true;
                        options.IncludeErrorDetails = true;

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var authHeader = context.Request.Headers.Authorization.ToString();
                                if (string.IsNullOrWhiteSpace(authHeader))
                                    return Task.CompletedTask;

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
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key))
                        };
                    });
            }

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Cau hinh middleware pipeline.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject.PersistAuthorization = true;
            });

            app.UseStaticFiles();
            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
