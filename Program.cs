using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain.Repositories;
using PicPaySimplificado.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
SQLitePCL.Batteries.Init();

// Add services to the container.
builder.Services.AddControllers();

// ✅ Configuração correta do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PicPaySimplificado API",
        Version = "v1"
    });
});

// ✅ Configuração do Banco de Dados SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Registro de dependências
builder.Services.AddScoped<ICommonUserRepository, CommonUserRepository>();
builder.Services.AddScoped<CommomUserService>();

builder.Services.AddScoped<ISellerUserRepository, SellerUserRepository>();
builder.Services.AddScoped<SellerUserService>();

var app = builder.Build();

// ✅ Habilitar Swagger apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicPaySimplificado API V1");
        c.RoutePrefix = string.Empty; // Permite acessar diretamente em http://localhost:5185/
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
