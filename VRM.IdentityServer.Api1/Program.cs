using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Здесь добавляем аутентификацию с помощью JWT токена
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", config =>
    {
        // Кто мы есть
        config.Audience = "ApiOne";
        // Адрес сервера аутентификации
        config.Authority = "https://localhost:5001";
    });

// Добавляем возможность использовать HttpClient
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VRM.IdentityServer.Api1", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VRM.IdentityServer.Api1 v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
