namespace KanbanApp.API.Contracts.ColumnsControllers
{
	// Представляет запрос на создание или обновление колонки в канбан-доске.
	public record ColumnsKanbanRequest(
		string Name, 
		Guid BoardId 
	);
}