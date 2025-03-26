using KanbanApp.Core.Model;
using KanbanApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.DataAccess.Repositories
{
	public class SubtaskKanbanRepository : ISubtasksKanbanRepository
	{
		private readonly KanbanAppDbContext _context;
		public SubtaskKanbanRepository(KanbanAppDbContext context)
		{
			_context = context;
		}

		public async Task<List<SubtaskKanban>> Get()
		{
			var subtaskEntities = await _context.Subtasks
				.AsNoTracking()
				.ToListAsync();

			var subtasks = subtaskEntities
				.Select(b => SubtaskKanban.Create(b.Id, b.Name, b.TaskId).SubtaskKanban)
				.ToList();

			return subtasks;
		}

		public async Task<Guid> Create(SubtaskKanban subtaskKanban)
		{
			var subtaskKanbanEntity = new SubtaskKanbanEntity
			{
				Id = subtaskKanban.Id,
				Name = subtaskKanban.Name,
				TaskId = subtaskKanban.TaskId
			};

			await _context.AddAsync(subtaskKanbanEntity);
			await _context.SaveChangesAsync();

			return subtaskKanbanEntity.Id;
		}

		public async Task<Guid> Update(Guid id, string name)
		{
			var subtaskEntity = await _context.Subtasks.FindAsync(id);
			if (subtaskEntity == null)
			{
				throw new InvalidOperationException($"Подзадача не найдена");
			}
			subtaskEntity.Name = name;
			await _context.SaveChangesAsync();

			return subtaskEntity.Id;
		}


		public async Task<Guid> Delete(Guid id)
		{
			await _context.Subtasks
				.Where(b => b.Id == id)
				.ExecuteDeleteAsync();
			return id;
		}

		public async Task<List<SubtaskKanban>> GetByTaskId(Guid id)
		{
			var subtasksEntities = await _context.Subtasks
				.AsNoTracking()
				.Where(b => b.TaskId == id)
				.ToListAsync();

			var subtasks = subtasksEntities
				.Select(b => SubtaskKanban.Create(b.Id, b.Name, b.TaskId).SubtaskKanban)
				.ToList();
			return subtasks;		
		}

	}
}
