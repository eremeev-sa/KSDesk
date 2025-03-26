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

var app = builder.Build();

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
        throw; // Если миграции не удалось применить, приложение не должно запускаться
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
            await context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error",
                Details = ex.Message,
                StackTrace = ex.StackTrace
            }.ToString());
        }
    });
});

// Настраиваем CORS
app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000", "https://kanban-frontend.onrender.com", "https://kanban-frontend-5fiz.onrender.com", "https://kanban-frontend-nlj3.onrender.com");
    x.WithMethods().AllowAnyMethod();
});

// Включаем Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanbanApp API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();