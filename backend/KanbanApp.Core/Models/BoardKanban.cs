namespace KanbanApp.Core.Model
{
	public class BoardKanban
	{
		public const int MAX_KANBANBOARD_NAME_LENGTH = 30;

		// Список колонок, принадлежащих доске
		public List<ColumnKanban> Columns { get; private set; }

		private BoardKanban(Guid id, string name)
		{
			Id = id;
			Name = name;
			Columns = new List<ColumnKanban>();
		}

		public Guid Id { get; }

		public string Name { get; set; } = string.Empty;

		public static (BoardKanban BoardKanban, string Error) Create(Guid id, string name)
		{
			var error = string.Empty;

			if (string.IsNullOrEmpty(name) || name.Length > MAX_KANBANBOARD_NAME_LENGTH)
			{
				error = "Поле не должно быть пустым или не должно превышать 30 символов";
			}

			var board = new BoardKanban(id, name);
			return (board, error);
		}
	}
}
