using KanbanApp.Core.Model;
using KanbanApp.Core.Models;

namespace KanbanApp.Application.Services
{
	public class SubtasksKanbanService : ISubtasksKanbanService
	{
		private readonly ISubtasksKanbanRepository _subtaskKanbanRepository;

		public SubtasksKanbanService(ISubtasksKanbanRepository subtaskKanbanRepository)
		{
			_subtaskKanbanRepository = subtaskKanbanRepository;
		}

		// Получение всех подзадач для конкретной задачи
		public async Task<List<SubtaskKanban>> GetAllSubtasksKanban()
		{
			return await _subtaskKanbanRepository.Get();
		}

		// Создание новой подзадачи
		public async Task<Guid> CreateSubtaskKanban(SubtaskKanban subtaskKanban)
		{
			return await _subtaskKanbanRepository.Create(subtaskKanban);
		}

		// Обновление подзадачизадачи
		public async Task<Guid> UpdateSubtaskKanban(Guid id, string name)
		{
			return await _subtaskKanbanRepository.Update(id, name);
		}

		// Удаление подзадачи
		public async Task<Guid> DeleteSubtaskKanban(Guid id)
		{
			return await _subtaskKanbanRepository.Delete(id);
		}

		// Получение подзадач по задаче
		public async Task<List<SubtaskKanban>> GetSubtasksByTaskId(Guid taskId)
		{
			return await _subtaskKanbanRepository.GetByTaskId(taskId);  // Вызов метода репозитория
		}
	}
}
