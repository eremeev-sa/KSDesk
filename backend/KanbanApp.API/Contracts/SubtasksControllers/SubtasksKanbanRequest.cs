namespace KanbanApp.API.Contracts.SubtasksControllers
{
	public record SubtasksKanbanRequest(
		string Name,
		Guid TaskId
	);
}
