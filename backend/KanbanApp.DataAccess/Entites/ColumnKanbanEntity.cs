namespace KanbanApp.DataAccess.Entites
{
	public class ColumnKanbanEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Order { get; set; } 
		public Guid BoardId { get; set; }
		public BoardKanbanEntity? Board { get; set; } 
		public List<TaskKanbanEntity> Tasks { get; set; } = new List<TaskKanbanEntity>();
	}
}
