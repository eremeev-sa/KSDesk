using KanbanApp.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.DataAccess.Repositories
{
	public class TaskKanbanRepository : ITasksKanbanRepository
	{
		private readonly KanbanAppDbContext _context;

		public TaskKanbanRepository(KanbanAppDbContext context)
		{
			_context = context;
		}

		// Получение всех задач
		public async Task<List<TaskKanban>> Get()
		{
			var taskEntities = await _context.Tasks
				.AsNoTracking()
				.ToListAsync();

			var tasks = taskEntities
				.Select(b => TaskKanban.Create(b.Id, b.Name, b.Description ?? string.Empty, b.Priority, b.ColumnId, b.AssignedId).TaskKanban)
				.Where(t => t != null)
				.Select(t => t!)
				.ToList();

			return tasks;
		}

		// Получение задач по колонке
		public async Task<List<TaskKanban>> GetByColumnId(Guid columnId)
		{
			var taskEntities = await _context.Tasks
				.AsNoTracking()
				.Where(b => b.ColumnId == columnId)
				.ToListAsync();  

			var tasks = taskEntities
				.Select(b => TaskKanban.Create(b.Id, b.Name, b.Description ?? string.Empty, b.Priority, b.ColumnId, b.AssignedId).TaskKanban)
				.Where(t => t != null)
				.Select(t => t!)
				.ToList();

			return tasks;
		}

		// Создание задачи
		public async Task<Guid> Create(TaskKanban taskKanban)
		{
			var taskKanbanEntity = new TaskKanbanEntity
			{
				Id = taskKanban.Id,
				Name = taskKanban.Name,
				Priority = taskKanban.Priority,
				Description = taskKanban.Description,
				ColumnId = taskKanban.ColumnId,
				AssignedId = taskKanban.AssignedId
			};

			await _context.Tasks.AddAsync(taskKanbanEntity);
			await _context.SaveChangesAsync();

			return taskKanbanEntity.Id;
		}

		// Обновление задачи
		public async Task<Guid> Update(Guid id, string name, string priority, string description, Guid? assignedId)
		{
			await _context.Tasks
				.Where(b => b.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(b => b.Name, b => name)
					.SetProperty(b => b.AssignedId, b => assignedId)
					.SetProperty(b => b.Priority, b => priority)
					.SetProperty(b => b.Description, b => description)
				);
			return id;
		}

		// Удаление задачи
		public async Task<Guid> Delete(Guid id)
		{
			await _context.Tasks
				.Where(b => b.Id == id)
				.ExecuteDeleteAsync();

			return id;
		}

		public async Task<Guid> UpdateTaskColumn(Guid id, Guid columnId)
		{
			await _context.Tasks
				.Where(b => b.Id == id)
				.ExecuteUpdateAsync(s => s
				.SetProperty(b => b.ColumnId, b => columnId)
			);
			return id;
		}

		public async Task<TaskKanban?> GetByIdTask(Guid id)
		{
			var taskEntity = await _context.Tasks
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == id);

			if (taskEntity == null)
			{
				return null;
			}

			var (taskKanban, error) = TaskKanban.Create(
				taskEntity.Id,
				taskEntity.Name,
				taskEntity.Description ?? string.Empty,
				taskEntity.Priority,
				taskEntity.ColumnId,
				taskEntity.AssignedId);

			return taskKanban;
		}

	}
}
