namespace KanbanApp.API.Contracts.BoardsControllers
{
	// Представляет запрос на создание или обновление канбан-доски с указанным именем.
	public record BoardsKanbanRequest(
		string Name
	);
}