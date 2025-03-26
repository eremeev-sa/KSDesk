using KanbanApp.Core.Model;

public interface ITasksKanbanRepository
{
	// Получение всех задач
	Task<List<TaskKanban>> Get();

	// Создание задачи
	Task<Guid> Create(TaskKanban taskKanban);

	// Обновление задачи
	Task<Guid> Update(Guid id, string name, string priority, string description, Guid? assignedId);

	// Удаление задачи
	Task<Guid> Delete(Guid id);

	// Получение задач по ColumnId
	Task<List<TaskKanban>> GetByColumnId(Guid columnId);

	Task<Guid> UpdateTaskColumn(Guid id, Guid columnId);

	Task<TaskKanban?> GetByIdTask(Guid id);
}
