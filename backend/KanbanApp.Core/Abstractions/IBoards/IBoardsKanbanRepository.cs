using KanbanApp.Core.Model;

namespace KanbanApp.Core.Abstractions.IBoards
{
    public interface IBoardsKanbanRepository
    {
        // Метод для получения всех канбан-досок
        Task<List<BoardKanban>> Get();

        // Метод для создания новой канбан-доски
        Task<Guid> Create(BoardKanban boardKanban);

        // Метод для обновления существующей канбан-доски по ID
        Task<Guid> Update(Guid id, string name);

        // Метод для удаления канбан-доски по ID
        Task<Guid> Delete(Guid id);
    }
}
