using KanbanApp.API.Contracts.ColumnsControllers;
using KanbanApp.Core.Abstractions.IBoards;
using KanbanApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.API.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class ColumnsKanbanController : ControllerBase
	{
		private readonly IColumnsKanbanService _columnsService;
		private readonly IBoardsKanbanService _boardService; // Для проверки доски
		private readonly ITasksKanbanService _tasksKanbanService;

		public ColumnsKanbanController(IColumnsKanbanService columnsService, IBoardsKanbanService boardService, ITasksKanbanService tasksKanbanService)
		{
			_columnsService = columnsService;
			_boardService = boardService;
			_tasksKanbanService = tasksKanbanService;
		}

		// Метод для получения всех колонок канбан-досок
		[HttpGet]
		public async Task<ActionResult<List<ColumnsKanbanResponse>>> GetColumns()
		{
			var columns = await _columnsService.GetAllColumnsKanban();
			var response = columns.Select(b => new ColumnsKanbanResponse(b.Id, b.Name));
			return Ok(response);
		}

		// Метод для создания новой колонки в канбан-доске
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateColumns([FromBody] ColumnsKanbanRequest request)
		{
			var columns = await _columnsService.GetColumnsByBoardId(request.BoardId);
			int maxOrder = columns.Any() ? columns.Max(c => c.Order) : 0;

			int newOrder = maxOrder + 1;

			(ColumnKanban column, string error) = ColumnKanban.Create(
				Guid.NewGuid(),
				request.Name,
				request.BoardId,
				newOrder 
			);

			if (!string.IsNullOrEmpty(error))
			{
				return BadRequest(error);
			}

			var columnId = await _columnsService.CreateColumnKanban(column);
			return Ok(columnId);
		}

		// Метод для обновления существующей колонки
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<Guid>> UpdateColumns(Guid id, [FromBody] ColumnsKanbanRequest request)
		{
			int order = 0; 
			var columnId = await _columnsService.UpdateColumnKanban(id, request.Name, order);
			return Ok(columnId);
		}

		// Метод для удаления колонки
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<Guid>> DeleteColumns(Guid id)
		{
			return Ok(await _columnsService.DeleteColumnKanban(id));
		}

		// Метод для обновления порядка колонок
		[HttpPut("order")]
		public async Task<ActionResult> UpdateColumnsOrder([FromBody] UpdateColumnsOrderRequest request)
		{
			await _columnsService.UpdateColumnsOrder(request.OrderedColumnIds);
			return Ok();
		}

		[HttpGet("{columnId}/tasks")]
		public async Task<ActionResult<List<TaskKanban>>> GetTasksByColumnId(Guid columnId)
		{
			var tasks = await _tasksKanbanService.GetTasksByColumnId(columnId);

			return Ok(tasks);
		}
	}
}
