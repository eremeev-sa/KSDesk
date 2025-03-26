using KanbanApp.Core.Abstractions.IBoards;
using KanbanApp.Core.Model;

namespace KanbanApp.Application.Services
{
    public class BoardsKanbanService : IBoardsKanbanService
	{
		private readonly IBoardsKanbanRepository _boardsKanbanRepository;

		public BoardsKanbanService(IBoardsKanbanRepository boardsKanbanRepository)
		{
			_boardsKanbanRepository = boardsKanbanRepository;
		}

		// Метод для получения всех канбан-досок
		public async Task<List<BoardKanban>> GetAllBoardsKanban()
		{
			return await _boardsKanbanRepository.Get();
		}

		// Метод для создания новой канбан-доски
		public async Task<Guid> CreateBoardKanban(BoardKanban boardKanban)
		{
			return await _boardsKanbanRepository.Create(boardKanban);
		}

		// Метод для обновления существующей канбан-доски
		public async Task<Guid> UpdateBoardKanban(Guid id, string name)
		{
			return await _boardsKanbanRepository.Update(id, name);
		}

		// Метод для удаления канбан-доски по ID
		public async Task<Guid> DeleteBoardKanban(Guid id)
		{
			return await _boardsKanbanRepository.Delete(id);
		}
	}
}
