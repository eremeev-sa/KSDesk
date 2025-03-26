using KanbanApp.API.Contracts.BoardsControllers;
using KanbanApp.API.Contracts.ColumnsControllers;
using KanbanApp.Core.Abstractions.IBoards;
using KanbanApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.API.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class BoardsKanbanController : ControllerBase
	{
		private readonly IBoardsKanbanService _boardsService;
		private readonly IColumnsKanbanService _columnsService;

		public BoardsKanbanController(IBoardsKanbanService boardsService, IColumnsKanbanService columnsService)
		{
			_boardsService = boardsService;
			_columnsService = columnsService;
		}

		// Метод для получения всех канбан-досок
		[HttpGet]
		public async Task<ActionResult<List<BoardsKanbanResponse>>> GetBoards()
		{
			var boards = await _boardsService.GetAllBoardsKanban();
			var responce = boards.Select(b => new BoardsKanbanResponse(b.Id, b.Name));
			return Ok(responce);
		}

		// Метод для создания новой канбан-доски
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateBoards([FromBody] BoardsKanbanRequest request)
		{
			(BoardKanban board, string error) = BoardKanban.Create(
				Guid.NewGuid(),
				request.Name);
			if (!string.IsNullOrEmpty(error))
			{
				return BadRequest(error);
			}
			var boardId = await _boardsService.CreateBoardKanban(board);
			return Ok(board);
		}

		// Метод для обновления существующей канбан-доски
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<Guid>> UpdateBoards(Guid id, [FromBody] BoardsKanbanRequest request)
		{
			var boardId = await _boardsService.UpdateBoardKanban(id, request.Name);
			return (boardId);
		}

		// Метод для удаления канбан-доски по ID
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<Guid>> DeleteBoards(Guid id)
		{
			return Ok(await _boardsService.DeleteBoardKanban(id));
		}

		// Метод для получения колонок по ID доски
		[HttpGet("{boardId:guid}/columns")]
		public async Task<ActionResult<List<ColumnsKanbanResponse>>> GetColumnsByBoardId(Guid boardId)
		{
			var columns = await _columnsService.GetColumnsByBoardId(boardId);
			var response = columns.Select(c => new ColumnsKanbanResponse(c.Id, c.Name));
			return Ok(response);
		}
	}
}
