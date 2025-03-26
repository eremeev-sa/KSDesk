namespace KanbanApp.API.Contracts.ColumnsControllers
{
	// Представляет ответ, содержащий данные о колонке канбан-доски: идентификатор и имя.
	public record ColumnsKanbanResponse(
		Guid Id,
		string Name
	);
}
