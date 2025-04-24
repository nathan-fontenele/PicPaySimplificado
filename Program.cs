using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Interfaces;
using PicPaySimplificado.Infrastructure;
using Transaction = PicPaySimplificado.Application.Transaction;
using Users = PicPaySimplificado.Application.Users;

var builder = WebApplication.CreateBuilder(args);
SQLitePCL.Batteries.Init();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PicPaySimplificado API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<Users>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<Transaction>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<ITransactionRepository<PicPaySimplificado.Domain.Transaction, Guid>, TransactionRepository>();

var app = builder.Build();

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
