using KanbanApp.API.Contracts.SubtasksControllers;
using KanbanApp.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SubtasksKanbanController : ControllerBase
	{
		private readonly ISubtasksKanbanService _subatasksService;

		public SubtasksKanbanController(ISubtasksKanbanService subtasksService)
		{
			_subatasksService = subtasksService;
		}

		//Метод для получение всех подзадач
		[HttpGet]
		public async Task<ActionResult<List<SubtasksKanbanResponce>>> GetSubtask()
		{
			var subtasks = await _subatasksService.GetAllSubtasksKanban();
			var responce = subtasks.Select(b => new SubtasksKanbanResponce(
				b.Id,
				b.Name
			));
			return Ok(responce);
		}

		//Метод для создания подзадачи
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateSubtask([FromBody] SubtasksKanbanRequest request)
		{
			(SubtaskKanban subtask, string error) = SubtaskKanban.Create(
				Guid.NewGuid(),
				request.Name,
				request.TaskId
			);
			if (!string.IsNullOrEmpty(error))
			{
				return BadRequest(error);
			}

			var subtaskId = await _subatasksService.CreateSubtaskKanban(subtask);
			return Ok(subtaskId);
		}

		//Метод для обновления подзадачи
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<Guid>> UpdateSubtask(Guid id, [FromBody] SubtasksUpdateRequest request)
		{
			var subtaskId = await _subatasksService.UpdateSubtaskKanban(
				id,
				request.Name
			);
			return Ok(subtaskId);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<Guid>> DeleteSubtask(Guid id)
		{
			return await _subatasksService.DeleteSubtaskKanban(id);
		}
	}
}
