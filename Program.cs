using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain.Repositories;
using PicPaySimplificado.Infrastructure;

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

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersRepository>();

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
