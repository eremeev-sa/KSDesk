using KanbanApp.Core.Model;
using KanbanApp.DataAccess.Entites;
using KanbanApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using KanbanApp.Core.Abstractions.IBoards;

public class BoardKanbanRepository : IBoardsKanbanRepository
{
	private readonly KanbanAppDbContext _context;

	public BoardKanbanRepository(KanbanAppDbContext context)
	{
		_context = context;
	}

	// Получение всех досок Kanban
	public async Task<List<BoardKanban>> Get()
	{
		var boardEntities = await _context.Boards
			.AsNoTracking()  // Отключение отслеживания изменений
			.ToListAsync();  // Асинхронная загрузка всех сущностей Board

		var boards = boardEntities
			.Select(b => BoardKanban.Create(b.Id, b.Name).BoardKanban)  // Преобразование
			.ToList();  // Список моделей данных

		return boards;  
	}

	// Создание новой доски Kanban
	public async Task<Guid> Create(BoardKanban boardKanban)
	{
		// Преобразование модели BoardKanban в сущность BoardKanbanEntity
		var boardKanbanEntity = new BoardKanbanEntity
		{
			Id = boardKanban.Id,  
			Name = boardKanban.Name, 
		};

		// Добавление сущности Board в контекст
		await _context.Boards.AddAsync(boardKanbanEntity); 
		await _context.SaveChangesAsync();  

		return boardKanbanEntity.Id;  
	}

	// Обновление данных доски Kanban
	public async Task<Guid> Update(Guid id, string name)
	{
		await _context.Boards
			.Where(b => b.Id == id)  
			.ExecuteUpdateAsync(s => s  
				.SetProperty(b => b.Name, b => name)  
			);
		return id;  
	}

	// Удаление доски Kanban
	public async Task<Guid> Delete(Guid id)
	{
		await _context.Boards
			.Where(b => b.Id == id)  
			.ExecuteDeleteAsync();  

		return id;  
	}
}
