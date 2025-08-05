using HumanDesignAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Human Design API", Version = "v1" });
});

// 注册我们的服务
builder.Services.AddScoped<IChartService, ChartService>();

// 添加CORS支持
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 添加日志
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// 添加根路径的健康检查
app.MapGet("/", () => new { 
    message = "Human Design API is running", 
    version = "1.0.0",
    timestamp = DateTime.UtcNow 
});

app.Run();  
