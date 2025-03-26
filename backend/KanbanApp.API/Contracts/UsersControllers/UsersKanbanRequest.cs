namespace KanbanApp.API.Contracts.UsersControllers
{
	// Представляет запрос на создание или обновление пользователя в системе канбан.
	public record UsersKanbanRequest(
		string Name,
		string Login,
		string Password
	);
}