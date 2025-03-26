namespace KanbanApp.DataAccess.Entites
{
	public class BoardKanbanEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public List<ColumnKanbanEntity> Columns { get; set; } = new List<ColumnKanbanEntity>();
	}
}
