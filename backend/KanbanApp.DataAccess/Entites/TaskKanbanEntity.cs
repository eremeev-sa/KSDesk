using KanbanApp.DataAccess.Entites;

public class TaskKanbanEntity
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string Priority { get; set; } = string.Empty;
	public Guid ColumnId { get; set; }
	public ColumnKanbanEntity? Column { get; set; }
	public Guid? AssignedId { get; set; }  
	public UserKanbanEntity? Assigned { get; set; }  
	public List<SubtaskKanbanEntity>? Subtasks { get; set; } = new List<SubtaskKanbanEntity>();
}
