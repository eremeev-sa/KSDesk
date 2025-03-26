using KanbanApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IColumnsKanbanService
{
	// Метод для получения всех колонок канбан-доски
	Task<List<ColumnKanban>> GetAllColumnsKanban();

	// Метод для создания новой колонки
	Task<Guid> CreateColumnKanban(ColumnKanban columnKanban);

	// Метод для обновления существующей колонки по ID
	Task<Guid> UpdateColumnKanban(Guid id, string name, int order);

	// Метод для удаления колонки по ID
	Task<Guid> DeleteColumnKanban(Guid id);

	// Метод для обновления порядка колонок
	Task UpdateColumnsOrder(List<Guid> orderedColumnIds);

	// Метод для получения колонок по ID доски
	Task<List<ColumnKanban>> GetColumnsByBoardId(Guid boardId);
}
