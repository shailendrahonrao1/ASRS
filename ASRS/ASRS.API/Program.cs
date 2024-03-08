using ASRS.Application.Interfaces;
using ASRS.Application.Services;
using ASRS.Application.Validators;
using ASRS.Application.ViewModels;
using ASRS.Core.Models;
using ASRS.Infrastructure.Data;
using ASRS.Infrastructure.Interfaces;
using ASRS.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASRSConnectionString"));
});

builder.Services.Configure<ClientConfiguration>(builder.Configuration.GetSection("ClientConfiguration"));
builder.Services.Configure<SqlConfiguration>(builder.Configuration.GetSection("SqlConfiguration"));
builder.Services.Configure<ExcelConfiguration>(builder.Configuration.GetSection("ExcelConfiguration"));

builder.Services.AddTransient<IAsrsService, AsrsService>();
builder.Services.AddScoped<IImportDataRepository, ImportDataRepository>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<AbstractValidator<ImportAsrsRequest>, ImportAsrsRequestValidators>();
builder.Services.AddSingleton<IValidator<ImportAsrsRequest>, ImportAsrsRequestValidators>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
