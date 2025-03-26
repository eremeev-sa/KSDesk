using KanbanApp.Core.Models;

public interface ISubtasksKanbanRepository
{
	// Получение всех подзадач
	Task<List<SubtaskKanban>> Get();

	// Создание подзадачи
	Task<Guid> Create(SubtaskKanban subtaskKanban);

	// Обновление подзадачи
	Task<Guid> Update(Guid id, string name);

	// Удаление подзадачи
	Task<Guid> Delete(Guid id);

	// Получение подзадач по TaskId
	Task<List<SubtaskKanban>> GetByTaskId(Guid taskId);
}
