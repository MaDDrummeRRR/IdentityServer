var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ����� ��������� �������������� � ������� JWT ������
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", config =>
    {
        // ��� �� ����
        config.Audience = "ApiOne";
        // ����� ������� ��������������
        config.Authority = "https://localhost:5001";
    });

// ��������� ����������� ������������ HttpClient
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Api1", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api1 v1"));
}

app.UseHttpsRedirection();

//����������� ���������� Middleware ��� ��������������
app.UseAuthentication();

//����������� ���������� Middleware ��� �����������
app.UseAuthorization();

app.MapControllers();

app.Run();
