using KanbanApp.API.Contracts.UsersControllers;
using KanbanApp.Core.Abstractions.IUsers;
using KanbanApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.API.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class UsersKanbanController : ControllerBase
	{
		private readonly IUsersKanbanService _usersService;

		public UsersKanbanController(IUsersKanbanService usersService)
		{
			_usersService = usersService;
		}

		// Метод для получения всех пользователей
		[HttpGet]
		public async Task<ActionResult<List<UsersKanbanResponse>>> GetUsers()
		{
			var users = await _usersService.GetAllUsers();
			var response = users.Select(b => new UsersKanbanResponse(b.Id, b.Name, b.Login, b.Password));
			return Ok(response);
		}

		// Метод для создания нового пользователя
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersKanbanRequest request)
		{
			(UserKanban? user, string error) = UserKanban.Create(
				Guid.NewGuid(),
				request.Name,
				request.Login,
				request.Password);

			if (user == null)
			{
				return BadRequest(error);
			}

			var userId = await _usersService.CreateUser(user);

			return Ok(userId);
		}

		// Метод для обновления данных пользователя
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<Guid>> UpdateUsers(Guid id, [FromBody] UsersKanbanRequest request)
		{
			
			if(string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
			{
				return BadRequest("Имя, ЛОгин, Пароль обязательны для заполнения");	
			}
			
			var userId = await _usersService.UpdateUser(id, request.Name, request.Login, request.Password);
			return Ok(userId);
		}

		// Метод для удаления пользователя
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<Guid>> DeleteUser(Guid id)
		{
			return Ok(await _usersService.DeleteUser(id));
		}
	}
}
