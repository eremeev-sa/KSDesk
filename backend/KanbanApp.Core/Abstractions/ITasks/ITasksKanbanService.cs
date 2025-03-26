using KanbanApp.Core.Model;

public interface ITasksKanbanService
{
	// Метод для создания новой задачи
	Task<Guid> CreateTaskKanban(TaskKanban taskKanban);

	// Метод для обновления задачи
	Task<Guid> UpdateTaskKanban(Guid id, string name, string priority, string description, Guid? assignedId);

	// Метод для удаления задачи по
	Task<Guid> DeleteTaskKanban(Guid id);

	// Метод для получения всех задач
	Task<List<TaskKanban>> GetAllTasksKanban();

	// Метод для получения задач по колонке
	Task<List<TaskKanban>> GetTasksByColumnId(Guid columnId);

	Task<Guid> UpdateTaskColumn(Guid id, Guid columnId);
}
