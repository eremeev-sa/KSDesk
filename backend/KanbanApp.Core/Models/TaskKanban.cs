namespace KanbanApp.Core.Model
{
	public class TaskKanban
	{
		public const int MAX_TASK_NAME_LENGTH = 50;

		private TaskKanban(Guid id, string name, string description, string priority, Guid columnId, Guid? assignedId)
		{
			Id = id;
			Name = name;
			Priority = priority;
			Description = description;
			ColumnId = columnId;
			AssignedId = assignedId;
		}

		public Guid Id { get; }

		public string Name { get; } = string.Empty;

		public string Description { get; }

		public string Priority { get; } = string.Empty;

		public Guid ColumnId { get; }

		public Guid? AssignedId { get; }

		public static (TaskKanban? TaskKanban, string Error) Create(Guid id, string name, string description, string priority, Guid columnId, Guid? assignedId)
		{
			var error = string.Empty;

			if (string.IsNullOrEmpty(name) || name.Length > MAX_TASK_NAME_LENGTH)
			{
				error = $"Поле не должно быть пустым или не должно превышать {MAX_TASK_NAME_LENGTH} символов";
			}

			if (columnId == Guid.Empty)
			{
				error = "ColumnId является обязательным и не может быть пустым.";
				return (null, error);
			}

			var task = new TaskKanban(id, name, description, priority, columnId, assignedId);
			return (task, error);
		}
	}
}
