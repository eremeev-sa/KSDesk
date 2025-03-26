using KanbanApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IColumnsKanbanRepository
{
	// Метод для получения всех колонок канбан-доски
	Task<List<ColumnKanban>> Get();

	// Метод для создания новой колонки
	Task<Guid> Create(ColumnKanban column);

	// Метод для обновления колонки по ID
	Task<Guid> Update(Guid id, string name, int order);

	// Метод для удаления колонки по ID
	Task<Guid> Delete(Guid id);

	// Метод для обновления порядка колонок
	Task UpdateOrder(List<Guid> orderedColumnIds);

	// Метод для получения всех колонок, привязанных к конкретной доске
	Task<List<ColumnKanban>> GetByBoardId(Guid boardId);
}
