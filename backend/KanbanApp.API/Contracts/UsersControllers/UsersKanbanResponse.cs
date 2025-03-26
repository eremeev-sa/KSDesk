namespace KanbanApp.API.Contracts.UsersControllers
{
	// Представляет ответ, содержащий данные о пользователе в системе канбан.
	public record UsersKanbanResponse(
		Guid Id,
		string Name,
		string Login,
		string Password
	);
}