using KanbanApp.Core.Model;

namespace KanbanApp.Core.Abstractions.IUsers
{
    public interface IUsersKanbanRepository
    {
        // Метод для получения всех пользователей
        Task<List<UserKanban>> Get();

        // Метод для создания нового пользователя
        Task<Guid> Create(UserKanban user);

        // Метод для обновления пользователя
        Task<Guid> Update(Guid id, string name, string login, string password);

        // Метод для удаления пользователя
        Task<Guid> Delete(Guid id);
    }
}
