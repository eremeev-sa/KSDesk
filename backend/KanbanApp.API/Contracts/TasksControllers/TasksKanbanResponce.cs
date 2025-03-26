namespace KanbanApp.API.Contracts.TasksControllers
{
	// Представляет ответ, содержащий данные о задаче в канбан-доске.
	public record TasksKanbanResponse(
		Guid Id,
		string Name,
		Guid AssignedId,
		string Priority,
		string Description
	);
}
