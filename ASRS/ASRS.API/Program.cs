using ASRS.API.Middleware;
using ASRS.Application.Interfaces;
using ASRS.Application.Profile;
using ASRS.Application.Services;
using ASRS.Application.Validators;
using ASRS.Application.ViewModels;
using ASRS.Core.Models;
using ASRS.Infrastructure.Data;
using ASRS.Infrastructure.Interfaces;
using ASRS.Infrastructure.Repositories;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ASRS Api",
        Version = "v1",
        Description = "This API is used to create and manage Store Receipts, Stock Release",
        Contact = new OpenApiContact
        {
            Email = "vguru.systems@gmail.com",
            Name = "IT Support vguru systems",
            Url = null
        }
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASRSConnectionString"));
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASRSConnectionString"));
});

builder.Services.Configure<ClientConfiguration>(builder.Configuration.GetSection("ClientConfiguration"));
builder.Services.Configure<SqlConfiguration>(builder.Configuration.GetSection("SqlConfiguration"));
builder.Services.Configure<ExcelConfiguration>(builder.Configuration.GetSection("ExcelConfiguration"));
builder.Services.AddAutoMapper(p => p.AddProfiles(new List<Profile>() { new DomainMapperProfile() }), typeof(Program));


builder.Services.AddTransient<IStoreReceiptService, StoreReceiptService>();
builder.Services.AddTransient<IStockReleaseService, StockReleaseService>();
builder.Services.AddScoped<IImportDataRepository, ImportDataRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
//builder.Services.Configure<ExitCodeConfig>(builder.Configuration.GetSection("ExitCodeConfig"));
builder.Services.AddSingleton<AbstractValidator<ImportAsrsRequest>, ImportAsrsRequestValidator>();
builder.Services.AddSingleton<IValidator<ImportAsrsRequest>, ImportAsrsRequestValidator>();
builder.Services.AddSingleton<AbstractValidator<StoreReceiptRequest>, StoreReceiptRequestValidator>();
builder.Services.AddSingleton<IValidator<StoreReceiptRequest>, StoreReceiptRequestValidator>();
builder.Services.AddSingleton<AbstractValidator<StockReleaseRequest>, StockReleaseRequestValidator>();
builder.Services.AddSingleton<IValidator<StockReleaseRequest>, StockReleaseRequestValidator>();
builder.Services.AddScoped<IStoreReceiptRepository, StoreReceiptRepository>();
builder.Services.AddScoped<IStockReleaseRepository, StockReleaseRepository>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("ASRS")
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASRS Api v1");
    //    c.RoutePrefix = string.Empty;
    //});
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
