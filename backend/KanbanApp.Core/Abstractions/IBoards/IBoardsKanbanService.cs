using KanbanApp.Core.Model;

namespace KanbanApp.Core.Abstractions.IBoards
{
    public interface IBoardsKanbanService
    {
        // Метод для создания новой канбан-доски
        Task<Guid> CreateBoardKanban(BoardKanban boardKanban);

        // Метод для обновления существующей канбан-доски по ID
        Task<Guid> UpdateBoardKanban(Guid id, string name);

        // Метод для удаления канбан-доски по ID
        Task<Guid> DeleteBoardKanban(Guid id);

        // Метод для получения всех канбан-досок
        Task<List<BoardKanban>> GetAllBoardsKanban();
    }
}
