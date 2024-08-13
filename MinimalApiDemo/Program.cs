using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiDemo.AutoMapper;
using MinimalApiDemo.DataAccess;
using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Repositories;
using MinimalApiDemo.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Minimal API Demo",
        Description = "AddAuthentication",
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

#region Authentication
app.MapPost("/Register", async (IAccountService accountService, RegisterDTO request) =>
{
    return Results.Ok(await accountService.Register(request));
}).WithTags("Authentication").AllowAnonymous();
app.MapPost("/Login", async (IAccountService accountService, LoginDTO request) =>
{
    return Results.Ok(await accountService.Login(request));
}).WithTags("Authentication").AllowAnonymous();
#endregion

#region Product
app.MapGet("/GetProduct", async (IProductService productService) =>
{
    return Results.Ok(await productService.GetAll());
}).WithTags("Product").RequireAuthorization();

app.MapGet("/GetProduct/{id:int}", async (IProductService productService, int id) =>
{
    return Results.Ok(await productService.GetById(id));
}).WithTags("Product").RequireAuthorization();

app.MapPost("/AddProduct", async (IProductService productService, AddRequestDTO request) =>
{
    return Results.Ok(await productService.Add(request));
}).WithTags("Product").RequireAuthorization();

app.MapPut("/UpdateProduct", async (IProductService productService, UpdateRequestDTO request) =>
{
    return Results.Ok(await productService.Update(request));
}).WithTags("Product").RequireAuthorization();

app.MapDelete("/DeleteProduct/{id:int}", async (IProductService productService, int id) =>
{
    return Results.Ok(await productService.Delete(id));
}).WithTags("Product").RequireAuthorization();
#endregion

app.Run();
