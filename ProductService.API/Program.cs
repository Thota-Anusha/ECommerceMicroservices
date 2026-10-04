using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProductService.Business.Excel;
using ProductService.Business.Interfaces;
using ProductService.Business.Services;
using ProductService.Data;
using ProductService.Data.Repositories.Implementations;
using ProductService.Data.Repositories.Interfaces;
using System.Text;

// Avoid namespace/class name conflict
using ProductServiceImplementation =
    ProductService.Business.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// CORS - Allow React Frontend
// ==================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(
                        origin,
                        UriKind.Absolute,
                        out var uri))
                {
                    return false;
                }

                return uri.Host.Equals(
                           "localhost",
                           StringComparison.OrdinalIgnoreCase)
                       || uri.Host.Equals(
                           "127.0.0.1");
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ==================================================
// JWT Authentication
// ==================================================

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!)
                    )
            };
    });

builder.Services.AddAuthorization();

// ==================================================
// Controllers
// ==================================================

builder.Services.AddControllers();

// ==================================================
// Database
// ==================================================

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString("DefaultConnection")
        )
);

// ==================================================
// Repository Dependency Injection
// ==================================================

builder.Services.AddScoped<
    IProductRepository,
    ProductRepository>();

builder.Services.AddScoped<
    ICategoryRepository,
    CategoryRepository>();

builder.Services.AddScoped<
    IInventoryRepository,
    InventoryRepository>();

// ==================================================
// Service Dependency Injection
// ==================================================

builder.Services.AddScoped<
    IProductService,
    ProductServiceImplementation>();

builder.Services.AddScoped<
    ICategoryService,
    CategoryService>();

builder.Services.AddScoped<
    IInventoryService,
    InventoryService>();

builder.Services.AddScoped<ProductExcelService>();

// ==================================================
// OpenAPI / Swagger
// ==================================================

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token"
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference(
                    "Bearer",
                    document
                ),
                new List<string>()
            }
        });
});

// ==================================================
// Build Application
// ==================================================

var app = builder.Build();

// ==================================================
// Swagger
// ==================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==================================================
// Middleware
// ==================================================

app.UseHttpsRedirection();

app.UseCors("ReactFrontend");

app.UseAuthentication();

app.UseAuthorization();

// ==================================================
// Map Controllers
// ==================================================

app.MapControllers();

// ==================================================
// Run Application
// ==================================================

app.Run();