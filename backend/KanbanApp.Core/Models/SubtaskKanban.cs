using KanbanApp.Core.Model;

namespace KanbanApp.Core.Models
{
	public class SubtaskKanban
	{
		public const int MAX_SUBTASK_NAME_LENGTH = 50;
		private SubtaskKanban(Guid id, string name, Guid taskId)
		{
			Id = id;
			Name = name;
			TaskId = taskId;
		}
		public Guid Id { get; }
		public string Name { get; } = string.Empty;
		public Guid TaskId { get; }
		public static (SubtaskKanban SubtaskKanban, string Error) Create(Guid id, string name, Guid taskId)
		{
			var error = string.Empty;

			if(string.IsNullOrEmpty(name) || name.Length > MAX_SUBTASK_NAME_LENGTH)
			{
				error = $"Поле не должно быть пустым или не должно превышать {MAX_SUBTASK_NAME_LENGTH}";
			}
			var subtask = new SubtaskKanban(id, name, taskId);
			return (subtask, error);

		}
	}
}
