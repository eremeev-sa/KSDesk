using KanbanApp.Application.Services;
using KanbanApp.Core.Abstractions.IBoards;
using KanbanApp.Core.Abstractions.IUsers;
using KanbanApp.DataAccess;
using KanbanApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования с помощью Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .MinimumLevel.Information();
});

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация DbContext
builder.Services.AddDbContext<KanbanAppDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

// Регистрация сервисов и репозиториев
builder.Services.AddScoped<IBoardsKanbanService, BoardsKanbanService>();
builder.Services.AddScoped<IColumnsKanbanService, ColumnsKanbanService>();
builder.Services.AddScoped<ITasksKanbanService, TasksKanbanService>();
builder.Services.AddScoped<IUsersKanbanService, UsersService>();
builder.Services.AddScoped<ISubtasksKanbanService, SubtasksKanbanService>();

builder.Services.AddScoped<IBoardsKanbanRepository, BoardKanbanRepository>();
builder.Services.AddScoped<IColumnsKanbanRepository, ColumnKanbanRepository>();
builder.Services.AddScoped<ITasksKanbanRepository, TaskKanbanRepository>();
builder.Services.AddScoped<IUsersKanbanRepository, UserRepository>();
builder.Services.AddScoped<ISubtasksKanbanRepository, SubtaskKanbanRepository>();

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Добавляем middleware для обработки OPTIONS-запросов и логирования
app.Use(async (context, next) =>
{
    Serilog.Log.Information("Request received: {Method} {Path} from {Origin}", context.Request.Method, context.Request.Path, context.Request.Headers["Origin"]);
    if (context.Request.Method == "OPTIONS")
    {
        Serilog.Log.Information("Handling OPTIONS request for {Path} from {Origin}", context.Request.Path, context.Request.Headers["Origin"]);
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
        context.Response.Headers["Access-Control-Max-Age"] = "86400";
        context.Response.StatusCode = 204;
        return;
    }
    await next();
});

// Применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<KanbanAppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        app.Logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}

// Настройка обработки ошибок
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error",
                Details = ex.Message,
                StackTrace = ex.StackTrace
            };
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorResponse));
        }
    });
});

// Настраиваем CORS (ДО UseAuthorization)
app.UseCors("AllowAll");

// Включаем Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanbanApp API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();

app.MapControllers();

// Добавляем эндпоинт для корневого пути
app.MapGet("/", () => "KanbanApp API is running!");

// Настраиваем порт
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();