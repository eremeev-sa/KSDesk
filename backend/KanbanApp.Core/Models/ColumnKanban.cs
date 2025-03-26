namespace KanbanApp.Core.Model
{
	public class ColumnKanban
	{
		public const int MAX_KANBANCOLUMN_NAME_LENGTH = 30;

		// Список задач, принадлежащих колонке
		public List<TaskKanban> Tasks { get; set; }

		// Порядок колонки на доске
		public int Order { get; private set; }

		private ColumnKanban(Guid id, string name, Guid boardId, int order)
		{
			Id = id;
			Name = name;
			BoardId = boardId;
			Order = order;
			Tasks = new List<TaskKanban>();
		}

		public Guid Id { get; }

		public string Name { get; } = string.Empty;

		public Guid BoardId { get; }

		public static (ColumnKanban ColumnKanban, string Error) Create(Guid id, string name, Guid boardId, int order)
		{
			var error = string.Empty;

			if (string.IsNullOrEmpty(name) || name.Length > MAX_KANBANCOLUMN_NAME_LENGTH)
			{
				error = "Поле не должно быть пустым или превышать 30 символов.";
			}

			var column = new ColumnKanban(id, name, boardId, order);
			return (column, error);
		}
	}
}
