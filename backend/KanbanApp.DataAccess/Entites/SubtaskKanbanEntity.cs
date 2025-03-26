public class SubtaskKanbanEntity
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Guid TaskId { get; set; }
	public TaskKanbanEntity? Task { get; set; }

}