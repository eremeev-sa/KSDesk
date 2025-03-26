namespace KanbanApp.API.Contracts.TasksControllers
{
	// Представляет запрос на создание или обновление задачи в канбан-доске.
	public record TasksKanbanRequest(
		string Name, 
		string Description, 
		string Priority, // Приоритет задачи (например, "High", "Medium", "Low")
		Guid AssignedId,
		Guid ColumnId
	);
}