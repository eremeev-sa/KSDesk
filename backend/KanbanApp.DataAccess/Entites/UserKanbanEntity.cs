using KanbanApp.Core.Model;

namespace KanbanApp.DataAccess.Entites
{
	public class UserKanbanEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Login { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public Permissions Permissions { get; set; }
		public List<TaskKanbanEntity> AssignedTasks { get; set; } = new List<TaskKanbanEntity>();
	}
}