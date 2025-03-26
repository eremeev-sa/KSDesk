using KanbanApp.API.Contracts.TasksControllers;
using KanbanApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TasksKanbanController : ControllerBase
	{
		private readonly ITasksKanbanService _tasksService;
		private readonly ISubtasksKanbanService _subtasksService;

		public TasksKanbanController(ITasksKanbanService tasksService, ISubtasksKanbanService subtasksService)
		{
			_tasksService = tasksService;
			_subtasksService = subtasksService;
		}

		// Метод для получения всех задач канбан-доски
		[HttpGet]
		public async Task<ActionResult<List<TasksKanbanResponse>>> GetTasks()
		{
			var tasks = await _tasksService.GetAllTasksKanban();
			if(tasks == null || !tasks.Any())
			{
				return NotFound("Задачи не найдены");
			}
			var response = tasks.Select(b => new TasksKanbanResponse(
				b.Id,
				b.Name,
				b.AssignedId ?? Guid.Empty, 
				b.Priority,
				b.Description ?? string.Empty
			));
			return Ok(response);
		}

		// Метод для создания новой задачи
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateTasks([FromBody] TasksKanbanRequest request)
		{
			Guid? assignedId = request.AssignedId == Guid.Empty ? (Guid?)null : request.AssignedId;

			var (task, error) = TaskKanban.Create(
				Guid.NewGuid(),
				request.Name,
				request.Description,
				request.Priority,
				request.ColumnId, 
				assignedId
			);

			if (!string.IsNullOrEmpty(error))
			{
				return BadRequest(error);
			}

			if(task == null)
			{
				return BadRequest("Ошибка при создании задачи");
			}

			var taskId = await _tasksService.CreateTaskKanban(task);
			return Ok(taskId);
		}

		// Метод для обновления существующей задачи
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<Guid>> UpdateTasks(Guid id, [FromBody] TasksUpdateRequest request)
		{
			var name = request.Name ?? null; 
			var priority = request.Priority ?? null; 
			var description = request.Description ?? null; 
			var assignedId = request.AssignedId ?? null;

			var taskId = await _tasksService.UpdateTaskKanban(
				id,
				name,
				priority,
				description,
				assignedId
			);
			return Ok(taskId);
		}

		// Метод для удаления задачи
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<Guid>> DeleteTask(Guid id)
		{
			return Ok(await _tasksService.DeleteTaskKanban(id));
		}

		[HttpGet("{taskId}/subtasks")]
		public async Task<ActionResult<List<TaskKanban>>> GetTasksByColumnId(Guid taskId)
		{
			var subtasks = await _subtasksService.GetSubtasksByTaskId(taskId);

			return Ok(subtasks);
		}
		
		[HttpPatch("{id:guid}/column")]
		public async Task<ActionResult<Guid>> UpdateTaskColumn(Guid id, [FromBody] Guid columnId)
		{
			if (columnId == Guid.Empty)
			{
				return BadRequest("ColumnId не может быть пустым.");
			}

			var updatedTaskId = await _tasksService.UpdateTaskColumn(id, columnId);

			return Ok(updatedTaskId);
		}
	}
}
