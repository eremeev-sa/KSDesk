namespace KanbanApp.API.Contracts.BoardsControllers
{
	// Представляет ответ, содержащий данные о канбан-доске: идентификатор и имя.
	public record BoardsKanbanResponse(
		Guid Id, 
		string Name
	);
}