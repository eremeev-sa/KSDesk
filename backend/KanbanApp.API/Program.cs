using KanbanApp.Application.Services;
using KanbanApp.Core.Abstractions.IBoards;
using KanbanApp.Core.Abstractions.IUsers;
using KanbanApp.DataAccess;
using KanbanApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KanbanAppDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

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

// Настраиваем CORS
app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

// Убираем проверку на IsDevelopment, чтобы Swagger был доступен всегда
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanbanApp API V1");
    c.RoutePrefix = string.Empty; // Делает Swagger доступным по корневому URL (http://localhost:5000/)
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();