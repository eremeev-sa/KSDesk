namespace KanbanApp.API.Contracts.UsersControllers
{
	public record UsersKanbanResponse(
		Guid Id,
		string Name,
		string Login,
		string Password
	);
}