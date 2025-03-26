namespace KanbanApp.API.Contracts.ColumnsControllers
{
	// Представляет запрос на обновление порядка колонок на канбан-доске.
	public record UpdateColumnsOrderRequest(
		List<Guid> OrderedColumnIds 
	);
}