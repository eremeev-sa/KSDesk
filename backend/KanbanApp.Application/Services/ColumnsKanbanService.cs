using KanbanApp.Core.Model;

public class ColumnsKanbanService : IColumnsKanbanService
{
	private readonly IColumnsKanbanRepository _columnsKanbanRepository;

	public ColumnsKanbanService(IColumnsKanbanRepository columnsKanbanRepository)
	{
		_columnsKanbanRepository = columnsKanbanRepository;
	}

	// Метод для получения всех колонок канбан-досок
	public async Task<List<ColumnKanban>> GetAllColumnsKanban()
	{
		return await _columnsKanbanRepository.Get();
	}

	// Метод для создания новой колонки в канбан-доске
	public async Task<Guid> CreateColumnKanban(ColumnKanban columnKanban)
	{
		return await _columnsKanbanRepository.Create(columnKanban);
	}

	// Метод для обновления существующей колонки в канбан-доске
	public async Task<Guid> UpdateColumnKanban(Guid id, string name, int order)
	{
		return await _columnsKanbanRepository.Update(id, name, order);
	}

	// Метод для удаления колонки по ID
	public async Task<Guid> DeleteColumnKanban(Guid id)
	{
		return await _columnsKanbanRepository.Delete(id);
	}

	// Метод для обновления порядка колонок
	public async Task UpdateColumnsOrder(List<Guid> orderedColumnIds)
	{
		await _columnsKanbanRepository.UpdateOrder(orderedColumnIds);
	}

	// Метод для получения колонок по ID доски
	public async Task<List<ColumnKanban>> GetColumnsByBoardId(Guid boardId)
	{
		return await _columnsKanbanRepository.GetByBoardId(boardId);
	}
}
