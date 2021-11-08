using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VRM.IdentityServer.Server.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

// Здесь добавляем DBContext, используем MSSQL Server в качестве БД
builder.Services.AddDbContext<ApplicationDBContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sql => sql.MigrationsAssembly(migrationsAssembly));
});

// Identity нужен для управления пользователями, логинами, паролями, клаймами
builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// Куки нужны для хранения токенов (в частности JWT токенов)
// Для коммуникации service-to-service это вообще не нужно
// Это нужно только для приложений, где есть некий фронт энд (MVC, Angular, Vue и прочее)
builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "Home/Login";
});

// Здесь добавляем IdentityServer4, который управляет разрешениями и валидациями
// Он выпускает токены и валидирует их, управляет разрешениями, для чего использует понятия такие как
// Client, ApiResource, IdentityResource, Scope
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    });

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VRM.IdentityServer.Server", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VRM.IdentityServer.Server v1"));
}

app.UseHttpsRedirection();

app.UseIdentityServer();

app.MapControllers();

app.Run();